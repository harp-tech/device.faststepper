using Bonsai.Harp;
using System.Threading;
using System.Threading.Tasks;

namespace Harp.FastStepper
{
    /// <inheritdoc/>
    public partial class Device
    {
        /// <summary>
        /// Initializes a new instance of the asynchronous API to configure and interface
        /// with FastStepper devices on the specified serial port.
        /// </summary>
        /// <param name="portName">
        /// The name of the serial port used to communicate with the Harp device.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous initialization operation. The value of
        /// the <see cref="Task{TResult}.Result"/> parameter contains a new instance of
        /// the <see cref="AsyncDevice"/> class.
        /// </returns>
        public static async Task<AsyncDevice> CreateAsync(string portName)
        {
            var device = new AsyncDevice(portName);
            var whoAmI = await device.ReadWhoAmIAsync();
            if (whoAmI != Device.WhoAmI)
            {
                var errorMessage = string.Format(
                    "The device ID {1} on {0} was unexpected. Check whether a FastStepper device is connected to the specified serial port.",
                    portName, whoAmI);
                throw new HarpException(errorMessage);
            }

            return device;
        }
    }

    /// <summary>
    /// Represents an asynchronous API to configure and interface with FastStepper devices.
    /// </summary>
    public partial class AsyncDevice : Bonsai.Harp.AsyncDevice
    {
        internal AsyncDevice(string portName)
            : base(portName)
        {
        }

        /// <summary>
        /// Asynchronously reads the contents of the Control register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ControlFlags> ReadControlAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Control.Address), cancellationToken);
            return Control.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Control register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ControlFlags>> ReadTimestampedControlAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(Control.Address), cancellationToken);
            return Control.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Control register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteControlAsync(ControlFlags value, CancellationToken cancellationToken = default)
        {
            var request = Control.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Encoder register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<short> ReadEncoderAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt16(Encoder.Address), cancellationToken);
            return Encoder.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Encoder register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<short>> ReadTimestampedEncoderAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt16(Encoder.Address), cancellationToken);
            return Encoder.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AnalogInput register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<short> ReadAnalogInputAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt16(AnalogInput.Address), cancellationToken);
            return AnalogInput.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AnalogInput register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<short>> ReadTimestampedAnalogInputAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt16(AnalogInput.Address), cancellationToken);
            return AnalogInput.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the StopSwitch register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<StopSwitchFlags> ReadStopSwitchAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StopSwitch.Address), cancellationToken);
            return StopSwitch.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the StopSwitch register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<StopSwitchFlags>> ReadTimestampedStopSwitchAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StopSwitch.Address), cancellationToken);
            return StopSwitch.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MotorBrake register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MotorBrakeFlags> ReadMotorBrakeAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MotorBrake.Address), cancellationToken);
            return MotorBrake.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MotorBrake register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MotorBrakeFlags>> ReadTimestampedMotorBrakeAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MotorBrake.Address), cancellationToken);
            return MotorBrake.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MotorBrake register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMotorBrakeAsync(MotorBrakeFlags value, CancellationToken cancellationToken = default)
        {
            var request = MotorBrake.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Moving register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<MovingFlags> ReadMovingAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Moving.Address), cancellationToken);
            return Moving.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Moving register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<MovingFlags>> ReadTimestampedMovingAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(Moving.Address), cancellationToken);
            return Moving.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the StopMovement register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadStopMovementAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StopMovement.Address), cancellationToken);
            return StopMovement.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the StopMovement register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedStopMovementAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(StopMovement.Address), cancellationToken);
            return StopMovement.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the StopMovement register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteStopMovementAsync(byte value, CancellationToken cancellationToken = default)
        {
            var request = StopMovement.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DirectVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadDirectVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(DirectVelocity.Address), cancellationToken);
            return DirectVelocity.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DirectVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedDirectVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(DirectVelocity.Address), cancellationToken);
            return DirectVelocity.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DirectVelocity register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDirectVelocityAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = DirectVelocity.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MoveTo register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadMoveToAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(MoveTo.Address), cancellationToken);
            return MoveTo.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MoveTo register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedMoveToAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(MoveTo.Address), cancellationToken);
            return MoveTo.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MoveTo register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMoveToAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = MoveTo.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MoveToEvents register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadMoveToEventsAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MoveToEvents.Address), cancellationToken);
            return MoveToEvents.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MoveToEvents register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedMoveToEventsAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(MoveToEvents.Address), cancellationToken);
            return MoveToEvents.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MinVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadMinVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(MinVelocity.Address), cancellationToken);
            return MinVelocity.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MinVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedMinVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(MinVelocity.Address), cancellationToken);
            return MinVelocity.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MinVelocity register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMinVelocityAsync(ushort value, CancellationToken cancellationToken = default)
        {
            var request = MinVelocity.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the MaxVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<ushort> ReadMaxVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(MaxVelocity.Address), cancellationToken);
            return MaxVelocity.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the MaxVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<ushort>> ReadTimestampedMaxVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt16(MaxVelocity.Address), cancellationToken);
            return MaxVelocity.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the MaxVelocity register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteMaxVelocityAsync(ushort value, CancellationToken cancellationToken = default)
        {
            var request = MaxVelocity.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Acceleration register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadAccelerationAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(Acceleration.Address), cancellationToken);
            return Acceleration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Acceleration register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedAccelerationAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(Acceleration.Address), cancellationToken);
            return Acceleration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Acceleration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAccelerationAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = Acceleration.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the Deceleration register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadDecelerationAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(Deceleration.Address), cancellationToken);
            return Deceleration.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the Deceleration register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedDecelerationAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(Deceleration.Address), cancellationToken);
            return Deceleration.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the Deceleration register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDecelerationAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = Deceleration.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the AccelerationJerk register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadAccelerationJerkAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(AccelerationJerk.Address), cancellationToken);
            return AccelerationJerk.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the AccelerationJerk register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedAccelerationJerkAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(AccelerationJerk.Address), cancellationToken);
            return AccelerationJerk.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the AccelerationJerk register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteAccelerationJerkAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = AccelerationJerk.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the DecelerationJerk register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadDecelerationJerkAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(DecelerationJerk.Address), cancellationToken);
            return DecelerationJerk.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the DecelerationJerk register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedDecelerationJerkAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(DecelerationJerk.Address), cancellationToken);
            return DecelerationJerk.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the DecelerationJerk register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteDecelerationJerkAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = DecelerationJerk.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the HomeSteps register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<int> ReadHomeStepsAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(HomeSteps.Address), cancellationToken);
            return HomeSteps.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the HomeSteps register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<int>> ReadTimestampedHomeStepsAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadInt32(HomeSteps.Address), cancellationToken);
            return HomeSteps.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the HomeSteps register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteHomeStepsAsync(int value, CancellationToken cancellationToken = default)
        {
            var request = HomeSteps.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the HomeStepsEvents register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<HomeStepsEventsFlags> ReadHomeStepsEventsAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(HomeStepsEvents.Address), cancellationToken);
            return HomeStepsEvents.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the HomeStepsEvents register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<HomeStepsEventsFlags>> ReadTimestampedHomeStepsEventsAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(HomeStepsEvents.Address), cancellationToken);
            return HomeStepsEvents.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the contents of the HomeVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<uint> ReadHomeVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt32(HomeVelocity.Address), cancellationToken);
            return HomeVelocity.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the HomeVelocity register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<uint>> ReadTimestampedHomeVelocityAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadUInt32(HomeVelocity.Address), cancellationToken);
            return HomeVelocity.GetTimestampedPayload(reply);
        }

        /// <summary>
        /// Asynchronously writes a value to the HomeVelocity register.
        /// </summary>
        /// <param name="value">The value to be stored in the register.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>The task object representing the asynchronous write operation.</returns>
        public async Task WriteHomeVelocityAsync(uint value, CancellationToken cancellationToken = default)
        {
            var request = HomeVelocity.FromPayload(MessageType.Write, value);
            await CommandAsync(request, cancellationToken);
        }

        /// <summary>
        /// Asynchronously reads the contents of the HomeSwitch register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the register payload.
        /// </returns>
        public async Task<byte> ReadHomeSwitchAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(HomeSwitch.Address), cancellationToken);
            return HomeSwitch.GetPayload(reply);
        }

        /// <summary>
        /// Asynchronously reads the timestamped contents of the HomeSwitch register.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The <see cref="Task{TResult}.Result"/>
        /// property contains the timestamped register payload.
        /// </returns>
        public async Task<Timestamped<byte>> ReadTimestampedHomeSwitchAsync(CancellationToken cancellationToken = default)
        {
            var reply = await CommandAsync(HarpCommand.ReadByte(HomeSwitch.Address), cancellationToken);
            return HomeSwitch.GetTimestampedPayload(reply);
        }
    }
}
