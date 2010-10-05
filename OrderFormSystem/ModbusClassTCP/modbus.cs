using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Net;
using System.Timers;
using System.Net.Sockets;
using System.Threading;

namespace ModbusTCP
{
	/// <summary>
	/// Modbus TCP common driver class. This class implements a modbus TCP master driver.
	/// It supports the following commands:
	/// 
	/// Read coils
	/// Read discrete inputs
	/// Write single coil
	/// Write multiple cooils
	/// Read holding register
	/// Read input register
	/// Write single register
	/// Write multiple register
	/// 
	/// All commands can be sent in synchronous or asynchronous mode. If a value is accessed
	/// in synchronous mode the program will stop and wait for slave to response. If the 
	/// slave didn't answer within a specified time a timeout exception is called.
	/// The class uses multi threading for both synchronous and asynchronous access. For
	/// the communication two lines are created. This is necessary because the synchronous
	/// thread has to wait for a previous command to finish.
	/// 
	/// </summary>
	public class Master
	{
		// ------------------------------------------------------------------------
		// Constants for access
		private const byte	fctReadCoil					= 1;
		private const byte	fctReadDiscreteInputs		= 2;
		private const byte	fctReadHoldingRegister		= 3;
		private const byte	fctReadInputRegister		= 4;
        private const byte fctWriteInputRegister = 4;
        private const byte  fctWriteHoldingRegister     = 16;
		private const byte	fctWriteSingleCoil			= 5;
		private const byte	fctWriteSingleRegister		= 6;
		private const byte	fctWriteMultipleCoils		= 15;
		private const byte	fctWriteMultipleRegister	= 16;

		/// <summary>Constant for exception illegal function.</summary>
		public const byte	excIllegalFunction			= 1;
		/// <summary>Constant for exception illegal data address.</summary>
		public const byte	excIllegalDataAdr			= 2;
		/// <summary>Constant for exception illegal data value.</summary>
		public const byte	excIllegalDataVal			= 3;
		/// <summary>Constant for exception slave device failure.</summary>
		public const byte	excSlaveDeviceFailure		= 4;
		/// <summary>Constant for exception acknoledge.</summary>
		public const byte	excAck						= 5;
		/// <summary>Constant for exception memory parity error.</summary>
		public const byte	excMemParityErr				= 6;
		/// <summary>Constant for exception gate path unavailable.</summary>
		public const byte	excGatePathUnavailable		= 10;
		/// <summary>Constant for exception not connected.</summary>
		public const byte	excExceptionNotConnected	= 253;
		/// <summary>Constant for exception connection lost.</summary>
		public const byte	excExceptionConnectionLost	= 254;
		/// <summary>Constant for exception response timeout.</summary>
		public const byte	excExceptionTimeout			= 255;

		private const byte	fctExceptionOffset			= 128;

		// ------------------------------------------------------------------------
		// Private declarations
		private static int		_timeout	= 500;
		private static bool		_connected  = false;
		private TcpClient		tcpAsyClient;
		private TcpClient		tcpSynClient;
		private ListenClass		thrAsyListen;
		private ListenClass		thrSynListen;

		// ------------------------------------------------------------------------
		/// <summary>Response data event. This event is called when new data arrives</summary>
		public delegate void			ResponseData(int id, byte function, byte[] data);
		/// <summary>Response data event. This event is called when new data arrives</summary>
		public event	ResponseData    OnResponseData;
		/// <summary>Exception data event. This event is called when the data is incorrect</summary>
		public delegate void			ExceptionData(int id, byte function, byte exception);
		/// <summary>Exception data event. This event is called when the data is incorrect</summary>
		public event	ExceptionData   OnException;

		// ------------------------------------------------------------------------
		/// <summary>Response timeout. If the slave didn't answers within in this time an exception is called.</summary>
		/// <value>The default value is 500ms.</value>
		public int timeout
		{
			get { return _timeout; }
			set { _timeout = value; }
		}

		// ------------------------------------------------------------------------
		/// <summary>Shows if a connection is active.</summary>
		public bool connected
		{
			get { return _connected; }
		}

		// ------------------------------------------------------------------------
		/// <summary>Create master instance.</summary>
		public Master()
		{
		}

		// ------------------------------------------------------------------------
		/// <summary>Create master instance.</summary>
		/// <param name="ip">IP adress of modbus slave.</param>
		/// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
		public Master(string ip, int port)
		{
			connect(ip, port);
		}

		// ------------------------------------------------------------------------
		/// <summary>Start connection to slave.</summary>
		/// <param name="ip">IP adress of modbus slave.</param>
		/// <param name="port">Port number of modbus slave. Usually port 502 is used.</param>
		public void connect(string ip, int port)
		{
			try
			{
				// ----------------------------------------------------------------
				// Connect asynchronous client
				tcpAsyClient = new TcpClient(ip, port);
				tcpAsyClient.ReceiveBufferSize	= 256;
				tcpAsyClient.SendBufferSize		= 256;
				// ----------------------------------------------------------------
				// Connect synchronous client
				tcpSynClient = new TcpClient(ip, port);
				tcpSynClient.ReceiveBufferSize	= 256;
				tcpSynClient.SendBufferSize		= 256;
				_connected = true;
			}
			catch(System.IO.IOException error)
			{
				_connected = false;
				throw(error);
			}
		}

		// ------------------------------------------------------------------------
		/// <summary>Stop connection to slave.</summary>
		public void disconnect()
		{
			Dispose();
		}

		// ------------------------------------------------------------------------
		/// <summary>Destroy master instance.</summary>
		~Master()
		{
			Dispose();
		}

		// ------------------------------------------------------------------------
		/// <summary>Destroy master instance</summary>
		public void Dispose()
		{
			if(tcpAsyClient != null)
			{
				tcpAsyClient.Close();
				tcpAsyClient = null;
			}
			if(tcpSynClient != null)
			{
				tcpSynClient.Close();
				tcpSynClient = null;
			}
			if(thrAsyListen != null)
			{
				thrAsyListen = null;
			}
			if(thrSynListen != null)
			{
				thrSynListen = null;
			}
		}

		// ------------------------------------------------------------------------
		/// <summary>Read coils from slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		public void ReadCoils(int id, int startAddress, byte numInputs)
		{
			WriteAsyncData(CreateReadHeader(id, startAddress, numInputs, fctReadCoil), id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Read coils from slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		/// <param name="values">Contains the result of function.</param>
		public void ReadCoils(int id, int startAddress, byte numInputs, ref byte[] values)
		{
			values = WriteSyncData(CreateReadHeader(id, startAddress, numInputs, fctReadCoil), id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Read discrete inputs from slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		public void ReadDiscreteInputs(int id, int startAddress, byte numInputs)
		{
			WriteAsyncData(CreateReadHeader(id, startAddress, numInputs, fctReadDiscreteInputs), id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Read discrete inputs from slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		/// <param name="values">Contains the result of function.</param>
		public void ReadDiscreteInputs(int id, int startAddress, byte numInputs, ref byte[] values)
		{
			values = WriteSyncData(CreateReadHeader(id, startAddress, numInputs, fctReadDiscreteInputs), id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Read holding registers from slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		public void ReadHoldingRegister(int id, int startAddress, byte numInputs)
		{
			WriteAsyncData(CreateReadHeader(id, startAddress, numInputs, fctReadHoldingRegister), id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Read holding registers from slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		/// <param name="values">Contains the result of function.</param>
		public void ReadHoldingRegister(int id, int startAddress, byte numInputs, ref byte[] values)
		{
			values = WriteSyncData(CreateReadHeader(id, startAddress, numInputs, fctReadHoldingRegister), id);
		}

        public void WriteHoldingRegister(int id, int startAddress, int numBits, byte[] values)
        {
            byte numBytes = Convert.ToByte(values.Length);
            byte[] data;
            data = CreateWriteHeader(id, startAddress, numBits, (byte)(numBytes + 2), fctWriteInputRegister);
            Array.Copy(values, 0, data, 13, numBytes);
            WriteAsyncData(data, id);
        }

		// ------------------------------------------------------------------------
		/// <summary>Read input registers from slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		public void ReadInputRegister(int id, int startAddress, byte numInputs)
		{
			WriteAsyncData(CreateReadHeader(id, startAddress, numInputs, fctReadInputRegister), id);
		}

        public void WriteInputRegister(int id, int startAddress, int numBits, byte[] values)
        {
            byte numBytes = Convert.ToByte(values.Length);
            byte[] data;
            data = CreateWriteHeader(id, startAddress, numBits, (byte)(numBytes + 2), fctWriteInputRegister);
            Array.Copy(values, 0, data, 13, numBytes);
            WriteAsyncData(data, id);
        }

        public void ReadRegister(int id, int startAddress, byte numInputs)
        {
            WriteAsyncData(CreateReadHeader(id, startAddress, numInputs, fctReadInputRegister), id);
        }

		// ------------------------------------------------------------------------
		/// <summary>Read input registers from slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numInputs">Length of data.</param>
		/// <param name="values">Contains the result of function.</param>
		public void ReadInputRegister(int id, int startAddress, byte numInputs, ref byte[] values)
		{
			values = WriteSyncData(CreateReadHeader(id, startAddress, numInputs, fctReadInputRegister), id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Write single coil in slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="OnOff">Specifys if the coil should be switched on or off.</param>
		public void WriteSingleCoils(int id, int startAddress, bool OnOff)
		{
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, 1, 1, fctWriteSingleCoil);
			if(OnOff == true)	data[10] = 255;
			else				data[10] = 0;
			WriteAsyncData(data, id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Write single coil in slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="OnOff">Specifys if the coil should be switched on or off.</param>
		/// <param name="result">Contains the result of the synchronous write.</param>
		public void WriteSingleCoils(int id, int startAddress, bool OnOff, ref byte[] result)
		{
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, 1, 1, fctWriteSingleCoil);
			if(OnOff == true)	data[10] = 255;
			else				data[10] = 0;
			result = WriteSyncData(data, id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Write multiple coils in slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numBits">Specifys number of bits.</param>
		/// <param name="values">Contains the bit information in byte format.</param>
		public void WriteMultipleCoils(int id, int startAddress, int numBits, byte[] values)
		{
			byte	numBytes = Convert.ToByte(values.Length);
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, numBits, (byte)(numBytes + 2), fctWriteMultipleCoils);
			Array.Copy(values, 0, data, 13, numBytes);
			WriteAsyncData(data, id);
		}

        public void _WriteMultipleCoils(int startAddress, int numBits, byte[] values)
        {
            byte numBytes = Convert.ToByte(values.Length);
            byte[] data;
            data = CreateWriteHeader(6, startAddress, numBits, (byte)(numBytes + 2), fctWriteMultipleCoils);
            Array.Copy(values, 0, data, 13, numBytes);
            WriteAsyncData(data, 6);
        }

		// ------------------------------------------------------------------------
		/// <summary>Write multiple coils in slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numBits">Specifys number of bits.</param>
		/// <param name="values">Contains the bit information in byte format.</param>
		/// <param name="result">Contains the result of the synchronous write.</param>
		public void WriteMultipleCoils(int id, int startAddress, int numBits, byte[] values, byte[] result)
		{
			byte	numBytes = Convert.ToByte(values.Length);
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, numBits, (byte)(numBytes + 2), fctWriteMultipleCoils);
			Array.Copy(values, 0, data, 13, numBytes);
			result = WriteSyncData(data, id);

		}

		// ------------------------------------------------------------------------
		/// <summary>Write single register in slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="values">Contains the register information.</param>
		public void WriteSingleRegister(int id, int startAddress, byte[] values)
		{
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, 1, 1, fctWriteSingleRegister);
			data[10] = values[0];
			data[11] = values[1];
			WriteAsyncData(data, id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Write single register in slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="values">Contains the register information.</param>
		/// <param name="result">Contains the result of the synchronous write.</param>
		public void WriteSingleRegister(int id, int startAddress, byte[] values, byte[] result)
		{
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, 1, 1, fctWriteSingleRegister);
			data[10] = values[0];
			data[11] = values[1];
			result = WriteSyncData(data, id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Write multiple registers in slave asynchronous. The result is given in the response function.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numRegs">Number of registers to be written.</param>
		/// <param name="values">Contains the register information.</param>
		public void WriteMultipleRegister(int id, int startAddress, int numRegs, byte[] values)
		{
			byte	numBytes = Convert.ToByte(values.Length);
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, numRegs, (byte)(numBytes + 2), fctWriteMultipleRegister);
			Array.Copy(values, 0, data, 13, numBytes);
			WriteAsyncData(data, id);
		}

		// ------------------------------------------------------------------------
		/// <summary>Write multiple registers in slave synchronous.</summary>
		/// <param name="id">Unique id that marks the transaction. In asynchonous mode this id is given to the callback function.</param>
		/// <param name="startAddress">Address from where the data read begins.</param>
		/// <param name="numRegs">Number of registers to be written.</param>
		/// <param name="values">Contains the register information.</param>
		/// <param name="result">Contains the result of the synchronous write.</param>
		public void WriteMultipleRegister(int id, int startAddress, int numRegs, byte[] values, byte[] result)
		{
			byte	numBytes = Convert.ToByte(values.Length);
			byte[]	data;
			data = CreateWriteHeader(id, startAddress, numRegs, (byte)(numBytes + 2), fctWriteMultipleRegister);
			Array.Copy(values, 0, data, 13, numBytes);
			result = WriteSyncData(data, id);
		}

		// ------------------------------------------------------------------------
		// Create modbus header for read action
		private byte[] CreateReadHeader(int id, int startAddress, byte length, byte function)
		{
			byte[]	data	= new byte[12];

			byte[] _id = BitConverter.GetBytes((short) id);
			data[0] = _id[0];				// Slave id high byte
			data[1] = _id[1];				// Slave id low byte
			data[5] = 6;					// Message size
			data[6] = 0;					// Slave address
			data[7] = function;				// Function code
			byte[] _adr = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) startAddress));
			data[8] = _adr[0];				// Start address
			data[9] = _adr[1];				// Start address
			data[11] = length;				// Number of data to read
			return data;
		}

		// ------------------------------------------------------------------------
		// Create modbus header for write action
		private byte[] CreateWriteHeader(int id, int startAddress, int numData, byte numBytes, byte function)
		{
			byte[]	data	= new byte[numBytes + 11];

			byte[] _id = BitConverter.GetBytes((short) id);
			data[0] = _id[0];				// Slave id high byte
			data[1] = _id[1];				// Slave id low byte+
			byte[] _size = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) (5 + numBytes)));
			data[4] = _size[0];				// Complete message size in bytes
			data[5] = _size[1];				// Complete message size in bytes
			data[6] = 1;					// Slave address
			data[7] = function;				// Function code
			byte[] _adr = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) startAddress));
			data[8] = _adr[0];				// Start address
			data[9] = _adr[1];				// Start address
			if(function >= fctWriteMultipleCoils)
			{
				byte[] cnt = BitConverter.GetBytes((short) IPAddress.HostToNetworkOrder((short) numData));
				data[10] = cnt[0];			// Number of bytes
				data[11] = cnt[1];			// Number of bytes
				data[12] = (byte)(numBytes - 2);
			}
			return data;
		}

		// ------------------------------------------------------------------------
		// Write data and create new asynchronous response thread
		private void WriteAsyncData(byte[] write_data, int id)
		{
			// --------------------------------------------------------------------
			// Create new asynchronous class
			thrAsyListen				= new ListenClass();
			thrAsyListen.OnException	+= new ModbusTCP.Master.ListenClass.ExceptionData(thrListen_OnException);
			thrAsyListen.OnResponseData	+= new ModbusTCP.Master.ListenClass.ResponseData(thrListen_OnResponseData);
			thrAsyListen.WriteData(tcpAsyClient, write_data, id, false);
		}

		// ------------------------------------------------------------------------
		// Write data and and wait for response
		private byte[] WriteSyncData(byte[] write_data, int id)
		{
			// --------------------------------------------------------------------
			// Wait until sync thread is free
			int time = 0;
			while((thrSynListen != null) && (time < _timeout))
			{
				Thread.Sleep(10);
                time += 10;
			}
			// --------------------------------------------------------------------
			// Create new synchronous task
			thrSynListen				= new ListenClass();
			thrSynListen.OnException	+= new ModbusTCP.Master.ListenClass.ExceptionData(thrListen_OnException);
			// --------------------------------------------------------------------
			// Response result if write request was ok
			if(thrSynListen.WriteData(tcpSynClient, write_data, id, true))
			{
				byte[] resp_data = thrSynListen.resp_data;
				thrSynListen = null;
				return resp_data;
			}
			// --------------------------------------------------------------------
			// Response null if write request was not ok
			thrSynListen = null;
			return null;
		}

		// ------------------------------------------------------------------------
		// Transfer event to class
		private void thrListen_OnException(int id, byte function, byte exception)
		{
			if(OnException != null) OnException(id, function, exception);
			if(exception == excExceptionConnectionLost)
			{
				_connected = false;
				tcpAsyClient.Close();
				tcpSynClient.Close();
				tcpAsyClient = null;
			}
		}

		// ------------------------------------------------------------------------
		// Transfer event to class
		private void thrListen_OnResponseData(int id, byte function, byte[] data)
		{
			if(OnResponseData != null) OnResponseData(id, function, data);
		}

		// ------------------------------------------------------------------------
		// Listen for incoming telegramm data asynchronous
		private class ListenClass
		{
			// --------------------------------------------------------------------
			internal delegate void			ResponseData(int id, byte function, byte[] data);
			internal event	ResponseData    OnResponseData;
			internal delegate void			ExceptionData(int id, byte function, byte exception);
			internal event	ExceptionData   OnException;
			
			private TcpClient	tcpClient;
			private bool		sync		= false;
			private int			req_id		= 0;
			public	byte[]		resp_data	= {};

			// --------------------------------------------------------------------
			public bool WriteData(TcpClient _tcpClient, byte[] write_data, int _req_id, bool _sync)
			{
				tcpClient	= _tcpClient;
				req_id		= _req_id;
				sync		= _sync;
				// ----------------------------------------------------------------
				// Send request to slave
				try
				{
					if(_connected) 
					{
						tcpClient.GetStream().Write(write_data, 0, write_data.Length);
						Thread thrListen = new Thread(new ThreadStart(ListenThread));
						thrListen.Start();
						if(sync == true) thrListen.Join();	
						return true;
					}
					else if(OnException != null) OnException(req_id, 0, excExceptionNotConnected);
				}
				// ----------------------------------------------------------------
				// Trap connection lost exception
				catch(Exception error)
				{
					if(error.InnerException.GetType() == typeof(System.Net.Sockets.SocketException))
						{
							if(OnException != null) OnException(req_id, 0, excExceptionConnectionLost);
						}
					else throw(error);
				}				
				return false;
			}

			// --------------------------------------------------------------------
			// Wait for slave response
			public void ListenThread()
			{
				int			time	= 0;
				byte[]		buffer	= new byte[256];
				int			id		= 0;
				byte[]		data;
				byte		function;
			
				while(time < _timeout)
				{
					// ----------------------------------------------------------------
					// Wait for new data
					if((tcpClient.GetStream().CanRead) &&
					   (tcpClient.GetStream().DataAvailable))
					{
						tcpClient.GetStream().Read(buffer, 0, buffer.GetUpperBound(0));
						id			= BitConverter.ToInt16(buffer, 0);
						function	= buffer[7];
						// ------------------------------------------------------------
						// Write response data
						if(function >= fctWriteSingleCoil)
						{
							data = new byte[2];
							Array.Copy(buffer, 10, data, 0, 2);
						}
							// ------------------------------------------------------------
							// Read response data
						else
						{
							data = new byte[buffer[8]];
							Array.Copy(buffer, 9, data, 0, buffer[8]);
						}
						// ------------------------------------------------------------
						// Response data is slave exception
						if(function > fctExceptionOffset)
						{
							function -= fctExceptionOffset;
							if(OnException != null) OnException(id, function, buffer[8]);
						}
							// ------------------------------------------------------------
							// Response data is regular data
						else if((OnResponseData != null) && (sync == false)) OnResponseData(id, function, data);
						else resp_data = data;
						break;
					}
					// ----------------------------------------------------------------
					// Retry reading every 10ms until timeout
					time += 10;
					Thread.Sleep(10);
				}
				if((time >= _timeout) && (OnException != null)) OnException(req_id, 0, excExceptionTimeout);
			}
		}
	}
}
