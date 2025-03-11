using Bonsai;
using Bonsai.Harp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Xml.Serialization;

namespace Harp.FastStepper
{
    /// <summary>
    /// Generates events and processes commands for the FastStepper device connected
    /// at the specified serial port.
    /// </summary>
    [Combinator(MethodName = nameof(Generate))]
    [WorkflowElementCategory(ElementCategory.Source)]
    [Description("Generates events and processes commands for the FastStepper device.")]
    public partial class Device : Bonsai.Harp.Device, INamedElement
    {
        /// <summary>
        /// Represents the unique identity class of the <see cref="FastStepper"/> device.
        /// This field is constant.
        /// </summary>
        public const int WhoAmI = 2120;

        /// <summary>
        /// Initializes a new instance of the <see cref="Device"/> class.
        /// </summary>
        public Device() : base(WhoAmI) { }

        string INamedElement.Name => nameof(FastStepper);

        /// <summary>
        /// Gets a read-only mapping from address to register type.
        /// </summary>
        public static new IReadOnlyDictionary<int, Type> RegisterMap { get; } = new Dictionary<int, Type>
            (Bonsai.Harp.Device.RegisterMap.ToDictionary(entry => entry.Key, entry => entry.Value))
        {
            { 32, typeof(Control) },
            { 33, typeof(Encoder) },
            { 34, typeof(AnalogInput) },
            { 35, typeof(StopSwitch) },
            { 36, typeof(MotorBrake) },
            { 37, typeof(Moving) },
            { 38, typeof(StopMovement) },
            { 39, typeof(DirectVelocity) },
            { 40, typeof(MoveTo) },
            { 41, typeof(MoveToEvents) },
            { 42, typeof(MinVelocity) },
            { 43, typeof(MaxVelocity) },
            { 44, typeof(Acceleration) },
            { 45, typeof(Deceleration) },
            { 46, typeof(AccelerationJerk) },
            { 47, typeof(DecelerationJerk) },
            { 48, typeof(HomeSteps) },
            { 49, typeof(HomeStepsEvents) },
            { 50, typeof(HomeVelocity) },
            { 51, typeof(HomeSwitch) }
        };
    }

    /// <summary>
    /// Represents an operator that groups the sequence of <see cref="FastStepper"/>" messages by register type.
    /// </summary>
    [Description("Groups the sequence of FastStepper messages by register type.")]
    public partial class GroupByRegister : Combinator<HarpMessage, IGroupedObservable<Type, HarpMessage>>
    {
        /// <summary>
        /// Groups an observable sequence of <see cref="FastStepper"/> messages
        /// by register type.
        /// </summary>
        /// <param name="source">The sequence of Harp device messages.</param>
        /// <returns>
        /// A sequence of observable groups, each of which corresponds to a unique
        /// <see cref="FastStepper"/> register.
        /// </returns>
        public override IObservable<IGroupedObservable<Type, HarpMessage>> Process(IObservable<HarpMessage> source)
        {
            return source.GroupBy(message => Device.RegisterMap[message.Address]);
        }
    }

    /// <summary>
    /// Represents an operator that filters register-specific messages
    /// reported by the <see cref="FastStepper"/> device.
    /// </summary>
    /// <seealso cref="Control"/>
    /// <seealso cref="Encoder"/>
    /// <seealso cref="AnalogInput"/>
    /// <seealso cref="StopSwitch"/>
    /// <seealso cref="MotorBrake"/>
    /// <seealso cref="Moving"/>
    /// <seealso cref="StopMovement"/>
    /// <seealso cref="DirectVelocity"/>
    /// <seealso cref="MoveTo"/>
    /// <seealso cref="MoveToEvents"/>
    /// <seealso cref="MinVelocity"/>
    /// <seealso cref="MaxVelocity"/>
    /// <seealso cref="Acceleration"/>
    /// <seealso cref="Deceleration"/>
    /// <seealso cref="AccelerationJerk"/>
    /// <seealso cref="DecelerationJerk"/>
    /// <seealso cref="HomeSteps"/>
    /// <seealso cref="HomeStepsEvents"/>
    /// <seealso cref="HomeVelocity"/>
    /// <seealso cref="HomeSwitch"/>
    [XmlInclude(typeof(Control))]
    [XmlInclude(typeof(Encoder))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(StopSwitch))]
    [XmlInclude(typeof(MotorBrake))]
    [XmlInclude(typeof(Moving))]
    [XmlInclude(typeof(StopMovement))]
    [XmlInclude(typeof(DirectVelocity))]
    [XmlInclude(typeof(MoveTo))]
    [XmlInclude(typeof(MoveToEvents))]
    [XmlInclude(typeof(MinVelocity))]
    [XmlInclude(typeof(MaxVelocity))]
    [XmlInclude(typeof(Acceleration))]
    [XmlInclude(typeof(Deceleration))]
    [XmlInclude(typeof(AccelerationJerk))]
    [XmlInclude(typeof(DecelerationJerk))]
    [XmlInclude(typeof(HomeSteps))]
    [XmlInclude(typeof(HomeStepsEvents))]
    [XmlInclude(typeof(HomeVelocity))]
    [XmlInclude(typeof(HomeSwitch))]
    [Description("Filters register-specific messages reported by the FastStepper device.")]
    public class FilterRegister : FilterRegisterBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterRegister"/> class.
        /// </summary>
        public FilterRegister()
        {
            Register = new Control();
        }

        string INamedElement.Name
        {
            get => $"{nameof(FastStepper)}.{GetElementDisplayName(Register)}";
        }
    }

    /// <summary>
    /// Represents an operator which filters and selects specific messages
    /// reported by the FastStepper device.
    /// </summary>
    /// <seealso cref="Control"/>
    /// <seealso cref="Encoder"/>
    /// <seealso cref="AnalogInput"/>
    /// <seealso cref="StopSwitch"/>
    /// <seealso cref="MotorBrake"/>
    /// <seealso cref="Moving"/>
    /// <seealso cref="StopMovement"/>
    /// <seealso cref="DirectVelocity"/>
    /// <seealso cref="MoveTo"/>
    /// <seealso cref="MoveToEvents"/>
    /// <seealso cref="MinVelocity"/>
    /// <seealso cref="MaxVelocity"/>
    /// <seealso cref="Acceleration"/>
    /// <seealso cref="Deceleration"/>
    /// <seealso cref="AccelerationJerk"/>
    /// <seealso cref="DecelerationJerk"/>
    /// <seealso cref="HomeSteps"/>
    /// <seealso cref="HomeStepsEvents"/>
    /// <seealso cref="HomeVelocity"/>
    /// <seealso cref="HomeSwitch"/>
    [XmlInclude(typeof(Control))]
    [XmlInclude(typeof(Encoder))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(StopSwitch))]
    [XmlInclude(typeof(MotorBrake))]
    [XmlInclude(typeof(Moving))]
    [XmlInclude(typeof(StopMovement))]
    [XmlInclude(typeof(DirectVelocity))]
    [XmlInclude(typeof(MoveTo))]
    [XmlInclude(typeof(MoveToEvents))]
    [XmlInclude(typeof(MinVelocity))]
    [XmlInclude(typeof(MaxVelocity))]
    [XmlInclude(typeof(Acceleration))]
    [XmlInclude(typeof(Deceleration))]
    [XmlInclude(typeof(AccelerationJerk))]
    [XmlInclude(typeof(DecelerationJerk))]
    [XmlInclude(typeof(HomeSteps))]
    [XmlInclude(typeof(HomeStepsEvents))]
    [XmlInclude(typeof(HomeVelocity))]
    [XmlInclude(typeof(HomeSwitch))]
    [XmlInclude(typeof(TimestampedControl))]
    [XmlInclude(typeof(TimestampedEncoder))]
    [XmlInclude(typeof(TimestampedAnalogInput))]
    [XmlInclude(typeof(TimestampedStopSwitch))]
    [XmlInclude(typeof(TimestampedMotorBrake))]
    [XmlInclude(typeof(TimestampedMoving))]
    [XmlInclude(typeof(TimestampedStopMovement))]
    [XmlInclude(typeof(TimestampedDirectVelocity))]
    [XmlInclude(typeof(TimestampedMoveTo))]
    [XmlInclude(typeof(TimestampedMoveToEvents))]
    [XmlInclude(typeof(TimestampedMinVelocity))]
    [XmlInclude(typeof(TimestampedMaxVelocity))]
    [XmlInclude(typeof(TimestampedAcceleration))]
    [XmlInclude(typeof(TimestampedDeceleration))]
    [XmlInclude(typeof(TimestampedAccelerationJerk))]
    [XmlInclude(typeof(TimestampedDecelerationJerk))]
    [XmlInclude(typeof(TimestampedHomeSteps))]
    [XmlInclude(typeof(TimestampedHomeStepsEvents))]
    [XmlInclude(typeof(TimestampedHomeVelocity))]
    [XmlInclude(typeof(TimestampedHomeSwitch))]
    [Description("Filters and selects specific messages reported by the FastStepper device.")]
    public partial class Parse : ParseBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Parse"/> class.
        /// </summary>
        public Parse()
        {
            Register = new Control();
        }

        string INamedElement.Name => $"{nameof(FastStepper)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents an operator which formats a sequence of values as specific
    /// FastStepper register messages.
    /// </summary>
    /// <seealso cref="Control"/>
    /// <seealso cref="Encoder"/>
    /// <seealso cref="AnalogInput"/>
    /// <seealso cref="StopSwitch"/>
    /// <seealso cref="MotorBrake"/>
    /// <seealso cref="Moving"/>
    /// <seealso cref="StopMovement"/>
    /// <seealso cref="DirectVelocity"/>
    /// <seealso cref="MoveTo"/>
    /// <seealso cref="MoveToEvents"/>
    /// <seealso cref="MinVelocity"/>
    /// <seealso cref="MaxVelocity"/>
    /// <seealso cref="Acceleration"/>
    /// <seealso cref="Deceleration"/>
    /// <seealso cref="AccelerationJerk"/>
    /// <seealso cref="DecelerationJerk"/>
    /// <seealso cref="HomeSteps"/>
    /// <seealso cref="HomeStepsEvents"/>
    /// <seealso cref="HomeVelocity"/>
    /// <seealso cref="HomeSwitch"/>
    [XmlInclude(typeof(Control))]
    [XmlInclude(typeof(Encoder))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(StopSwitch))]
    [XmlInclude(typeof(MotorBrake))]
    [XmlInclude(typeof(Moving))]
    [XmlInclude(typeof(StopMovement))]
    [XmlInclude(typeof(DirectVelocity))]
    [XmlInclude(typeof(MoveTo))]
    [XmlInclude(typeof(MoveToEvents))]
    [XmlInclude(typeof(MinVelocity))]
    [XmlInclude(typeof(MaxVelocity))]
    [XmlInclude(typeof(Acceleration))]
    [XmlInclude(typeof(Deceleration))]
    [XmlInclude(typeof(AccelerationJerk))]
    [XmlInclude(typeof(DecelerationJerk))]
    [XmlInclude(typeof(HomeSteps))]
    [XmlInclude(typeof(HomeStepsEvents))]
    [XmlInclude(typeof(HomeVelocity))]
    [XmlInclude(typeof(HomeSwitch))]
    [Description("Formats a sequence of values as specific FastStepper register messages.")]
    public partial class Format : FormatBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Format"/> class.
        /// </summary>
        public Format()
        {
            Register = new Control();
        }

        string INamedElement.Name => $"{nameof(FastStepper)}.{GetElementDisplayName(Register)}";
    }

    /// <summary>
    /// Represents a register that control the device modules.
    /// </summary>
    [Description("Control the device modules.")]
    public partial class Control
    {
        /// <summary>
        /// Represents the address of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const int Address = 32;

        /// <summary>
        /// Represents the payload type of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Control"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ControlFlags GetPayload(HarpMessage message)
        {
            return (ControlFlags)message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Control"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControlFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadUInt16();
            return Timestamped.Create((ControlFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Control"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Control"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ControlFlags value)
        {
            return HarpMessage.FromUInt16(Address, messageType, (ushort)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Control"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Control"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ControlFlags value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, (ushort)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Control register.
    /// </summary>
    /// <seealso cref="Control"/>
    [Description("Filters and selects timestamped messages from the Control register.")]
    public partial class TimestampedControl
    {
        /// <summary>
        /// Represents the address of the <see cref="Control"/> register. This field is constant.
        /// </summary>
        public const int Address = Control.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Control"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ControlFlags> GetPayload(HarpMessage message)
        {
            return Control.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the reading of the quadrature encoder.
    /// </summary>
    [Description("Contains the reading of the quadrature encoder.")]
    public partial class Encoder
    {
        /// <summary>
        /// Represents the address of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const int Address = 33;

        /// <summary>
        /// Represents the payload type of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Encoder"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static short GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Encoder"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Encoder"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Encoder"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Encoder"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Encoder"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Encoder register.
    /// </summary>
    /// <seealso cref="Encoder"/>
    [Description("Filters and selects timestamped messages from the Encoder register.")]
    public partial class TimestampedEncoder
    {
        /// <summary>
        /// Represents the address of the <see cref="Encoder"/> register. This field is constant.
        /// </summary>
        public const int Address = Encoder.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Encoder"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetPayload(HarpMessage message)
        {
            return Encoder.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the reading of the analog input.
    /// </summary>
    [Description("Contains the reading of the analog input.")]
    public partial class AnalogInput
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const int Address = 34;

        /// <summary>
        /// Represents the payload type of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S16;

        /// <summary>
        /// Represents the length of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AnalogInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static short GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AnalogInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AnalogInput"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogInput"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AnalogInput"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AnalogInput"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, short value)
        {
            return HarpMessage.FromInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AnalogInput register.
    /// </summary>
    /// <seealso cref="AnalogInput"/>
    [Description("Filters and selects timestamped messages from the AnalogInput register.")]
    public partial class TimestampedAnalogInput
    {
        /// <summary>
        /// Represents the address of the <see cref="AnalogInput"/> register. This field is constant.
        /// </summary>
        public const int Address = AnalogInput.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AnalogInput"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<short> GetPayload(HarpMessage message)
        {
            return AnalogInput.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the state of the stop switch.
    /// </summary>
    [Description("Contains the state of the stop switch.")]
    public partial class StopSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = 35;

        /// <summary>
        /// Represents the payload type of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StopSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static StopSwitchFlags GetPayload(HarpMessage message)
        {
            return (StopSwitchFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StopSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StopSwitchFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((StopSwitchFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StopSwitch"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StopSwitch"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, StopSwitchFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StopSwitch"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StopSwitch"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, StopSwitchFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StopSwitch register.
    /// </summary>
    /// <seealso cref="StopSwitch"/>
    [Description("Filters and selects timestamped messages from the StopSwitch register.")]
    public partial class TimestampedStopSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="StopSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = StopSwitch.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StopSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<StopSwitchFlags> GetPayload(HarpMessage message)
        {
            return StopSwitch.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the state of the motor brake output.
    /// </summary>
    [Description("Sets the state of the motor brake output.")]
    public partial class MotorBrake
    {
        /// <summary>
        /// Represents the address of the <see cref="MotorBrake"/> register. This field is constant.
        /// </summary>
        public const int Address = 36;

        /// <summary>
        /// Represents the payload type of the <see cref="MotorBrake"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MotorBrake"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MotorBrake"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MotorBrakeFlags GetPayload(HarpMessage message)
        {
            return (MotorBrakeFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MotorBrake"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MotorBrakeFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MotorBrakeFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MotorBrake"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MotorBrake"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MotorBrakeFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MotorBrake"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MotorBrake"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MotorBrakeFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MotorBrake register.
    /// </summary>
    /// <seealso cref="MotorBrake"/>
    [Description("Filters and selects timestamped messages from the MotorBrake register.")]
    public partial class TimestampedMotorBrake
    {
        /// <summary>
        /// Represents the address of the <see cref="MotorBrake"/> register. This field is constant.
        /// </summary>
        public const int Address = MotorBrake.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MotorBrake"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MotorBrakeFlags> GetPayload(HarpMessage message)
        {
            return MotorBrake.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the state of the motor movement.
    /// </summary>
    [Description("Contains the state of the motor movement.")]
    public partial class Moving
    {
        /// <summary>
        /// Represents the address of the <see cref="Moving"/> register. This field is constant.
        /// </summary>
        public const int Address = 37;

        /// <summary>
        /// Represents the payload type of the <see cref="Moving"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="Moving"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Moving"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static MovingFlags GetPayload(HarpMessage message)
        {
            return (MovingFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Moving"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MovingFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((MovingFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Moving"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Moving"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, MovingFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Moving"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Moving"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, MovingFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Moving register.
    /// </summary>
    /// <seealso cref="Moving"/>
    [Description("Filters and selects timestamped messages from the Moving register.")]
    public partial class TimestampedMoving
    {
        /// <summary>
        /// Represents the address of the <see cref="Moving"/> register. This field is constant.
        /// </summary>
        public const int Address = Moving.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Moving"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<MovingFlags> GetPayload(HarpMessage message)
        {
            return Moving.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that instantly stops the motor movement.
    /// </summary>
    [Description("Instantly stops the motor movement.")]
    public partial class StopMovement
    {
        /// <summary>
        /// Represents the address of the <see cref="StopMovement"/> register. This field is constant.
        /// </summary>
        public const int Address = 38;

        /// <summary>
        /// Represents the payload type of the <see cref="StopMovement"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="StopMovement"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="StopMovement"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="StopMovement"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="StopMovement"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StopMovement"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="StopMovement"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="StopMovement"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// StopMovement register.
    /// </summary>
    /// <seealso cref="StopMovement"/>
    [Description("Filters and selects timestamped messages from the StopMovement register.")]
    public partial class TimestampedStopMovement
    {
        /// <summary>
        /// Represents the address of the <see cref="StopMovement"/> register. This field is constant.
        /// </summary>
        public const int Address = StopMovement.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="StopMovement"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return StopMovement.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that instantly start moving at a specific speed and direction according to the register's value and signal.
    /// </summary>
    [Description("Instantly start moving at a specific speed and direction according to the register's value and signal.")]
    public partial class DirectVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="DirectVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = 39;

        /// <summary>
        /// Represents the payload type of the <see cref="DirectVelocity"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="DirectVelocity"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DirectVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DirectVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DirectVelocity"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DirectVelocity"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DirectVelocity"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DirectVelocity"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DirectVelocity register.
    /// </summary>
    /// <seealso cref="DirectVelocity"/>
    [Description("Filters and selects timestamped messages from the DirectVelocity register.")]
    public partial class TimestampedDirectVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="DirectVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = DirectVelocity.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DirectVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return DirectVelocity.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that moves to a specific position, using the velocity, acceleration and jerk configurations.
    /// </summary>
    [Description("Moves to a specific position, using the velocity, acceleration and jerk configurations.")]
    public partial class MoveTo
    {
        /// <summary>
        /// Represents the address of the <see cref="MoveTo"/> register. This field is constant.
        /// </summary>
        public const int Address = 40;

        /// <summary>
        /// Represents the payload type of the <see cref="MoveTo"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="MoveTo"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MoveTo"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MoveTo"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MoveTo"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MoveTo"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MoveTo"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MoveTo"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MoveTo register.
    /// </summary>
    /// <seealso cref="MoveTo"/>
    [Description("Filters and selects timestamped messages from the MoveTo register.")]
    public partial class TimestampedMoveTo
    {
        /// <summary>
        /// Represents the address of the <see cref="MoveTo"/> register. This field is constant.
        /// </summary>
        public const int Address = MoveTo.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MoveTo"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return MoveTo.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
    /// </summary>
    [Description("Reports possible events regarding the execution of the ADD_REG_MOVE_TO register.")]
    public partial class MoveToEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="MoveToEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = 41;

        /// <summary>
        /// Represents the payload type of the <see cref="MoveToEvents"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="MoveToEvents"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MoveToEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MoveToEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MoveToEvents"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MoveToEvents"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MoveToEvents"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MoveToEvents"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MoveToEvents register.
    /// </summary>
    /// <seealso cref="MoveToEvents"/>
    [Description("Filters and selects timestamped messages from the MoveToEvents register.")]
    public partial class TimestampedMoveToEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="MoveToEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = MoveToEvents.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MoveToEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return MoveToEvents.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the minimum velocity for the movement (steps/s).
    /// </summary>
    [Description("Sets the minimum velocity for the movement (steps/s)")]
    public partial class MinVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="MinVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = 42;

        /// <summary>
        /// Represents the payload type of the <see cref="MinVelocity"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="MinVelocity"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MinVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MinVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MinVelocity"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MinVelocity"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MinVelocity"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MinVelocity"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MinVelocity register.
    /// </summary>
    /// <seealso cref="MinVelocity"/>
    [Description("Filters and selects timestamped messages from the MinVelocity register.")]
    public partial class TimestampedMinVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="MinVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = MinVelocity.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MinVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return MinVelocity.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the maximum velocity for the movement (steps/s).
    /// </summary>
    [Description("Sets the maximum velocity for the movement (steps/s)")]
    public partial class MaxVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="MaxVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = 43;

        /// <summary>
        /// Represents the payload type of the <see cref="MaxVelocity"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U16;

        /// <summary>
        /// Represents the length of the <see cref="MaxVelocity"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="MaxVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static ushort GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt16();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="MaxVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt16();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="MaxVelocity"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MaxVelocity"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="MaxVelocity"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="MaxVelocity"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, ushort value)
        {
            return HarpMessage.FromUInt16(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// MaxVelocity register.
    /// </summary>
    /// <seealso cref="MaxVelocity"/>
    [Description("Filters and selects timestamped messages from the MaxVelocity register.")]
    public partial class TimestampedMaxVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="MaxVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = MaxVelocity.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="MaxVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<ushort> GetPayload(HarpMessage message)
        {
            return MaxVelocity.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the acceleration for the movement (steps/s^2).
    /// </summary>
    [Description("Sets the acceleration for the movement (steps/s^2)")]
    public partial class Acceleration
    {
        /// <summary>
        /// Represents the address of the <see cref="Acceleration"/> register. This field is constant.
        /// </summary>
        public const int Address = 44;

        /// <summary>
        /// Represents the payload type of the <see cref="Acceleration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="Acceleration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Acceleration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Acceleration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Acceleration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Acceleration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Acceleration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Acceleration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Acceleration register.
    /// </summary>
    /// <seealso cref="Acceleration"/>
    [Description("Filters and selects timestamped messages from the Acceleration register.")]
    public partial class TimestampedAcceleration
    {
        /// <summary>
        /// Represents the address of the <see cref="Acceleration"/> register. This field is constant.
        /// </summary>
        public const int Address = Acceleration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Acceleration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return Acceleration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the deceleration for the movement (steps/s^2).
    /// </summary>
    [Description("Sets the deceleration for the movement (steps/s^2)")]
    public partial class Deceleration
    {
        /// <summary>
        /// Represents the address of the <see cref="Deceleration"/> register. This field is constant.
        /// </summary>
        public const int Address = 45;

        /// <summary>
        /// Represents the payload type of the <see cref="Deceleration"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="Deceleration"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="Deceleration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="Deceleration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="Deceleration"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Deceleration"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="Deceleration"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="Deceleration"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// Deceleration register.
    /// </summary>
    /// <seealso cref="Deceleration"/>
    [Description("Filters and selects timestamped messages from the Deceleration register.")]
    public partial class TimestampedDeceleration
    {
        /// <summary>
        /// Represents the address of the <see cref="Deceleration"/> register. This field is constant.
        /// </summary>
        public const int Address = Deceleration.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="Deceleration"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return Deceleration.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the jerk for the acceleration part of the movement (steps/s^3).
    /// </summary>
    [Description("Sets the jerk for the acceleration part of the movement (steps/s^3)")]
    public partial class AccelerationJerk
    {
        /// <summary>
        /// Represents the address of the <see cref="AccelerationJerk"/> register. This field is constant.
        /// </summary>
        public const int Address = 46;

        /// <summary>
        /// Represents the payload type of the <see cref="AccelerationJerk"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="AccelerationJerk"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="AccelerationJerk"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="AccelerationJerk"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="AccelerationJerk"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AccelerationJerk"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="AccelerationJerk"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="AccelerationJerk"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// AccelerationJerk register.
    /// </summary>
    /// <seealso cref="AccelerationJerk"/>
    [Description("Filters and selects timestamped messages from the AccelerationJerk register.")]
    public partial class TimestampedAccelerationJerk
    {
        /// <summary>
        /// Represents the address of the <see cref="AccelerationJerk"/> register. This field is constant.
        /// </summary>
        public const int Address = AccelerationJerk.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="AccelerationJerk"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return AccelerationJerk.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the jerk for the deceleration part of the movement (steps/s^3).
    /// </summary>
    [Description("Sets the jerk for the deceleration part of the movement (steps/s^3)")]
    public partial class DecelerationJerk
    {
        /// <summary>
        /// Represents the address of the <see cref="DecelerationJerk"/> register. This field is constant.
        /// </summary>
        public const int Address = 47;

        /// <summary>
        /// Represents the payload type of the <see cref="DecelerationJerk"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="DecelerationJerk"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="DecelerationJerk"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="DecelerationJerk"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="DecelerationJerk"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DecelerationJerk"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="DecelerationJerk"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="DecelerationJerk"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// DecelerationJerk register.
    /// </summary>
    /// <seealso cref="DecelerationJerk"/>
    [Description("Filters and selects timestamped messages from the DecelerationJerk register.")]
    public partial class TimestampedDecelerationJerk
    {
        /// <summary>
        /// Represents the address of the <see cref="DecelerationJerk"/> register. This field is constant.
        /// </summary>
        public const int Address = DecelerationJerk.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="DecelerationJerk"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return DecelerationJerk.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
    /// </summary>
    [Description("Moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.")]
    public partial class HomeSteps
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeSteps"/> register. This field is constant.
        /// </summary>
        public const int Address = 48;

        /// <summary>
        /// Represents the payload type of the <see cref="HomeSteps"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.S32;

        /// <summary>
        /// Represents the length of the <see cref="HomeSteps"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="HomeSteps"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static int GetPayload(HarpMessage message)
        {
            return message.GetPayloadInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="HomeSteps"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="HomeSteps"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeSteps"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="HomeSteps"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeSteps"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, int value)
        {
            return HarpMessage.FromInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// HomeSteps register.
    /// </summary>
    /// <seealso cref="HomeSteps"/>
    [Description("Filters and selects timestamped messages from the HomeSteps register.")]
    public partial class TimestampedHomeSteps
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeSteps"/> register. This field is constant.
        /// </summary>
        public const int Address = HomeSteps.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="HomeSteps"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<int> GetPayload(HarpMessage message)
        {
            return HomeSteps.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that reports possible events regarding the execution of the REG_HOME_STEPS register.
    /// </summary>
    [Description("Reports possible events regarding the execution of the REG_HOME_STEPS register.")]
    public partial class HomeStepsEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeStepsEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = 49;

        /// <summary>
        /// Represents the payload type of the <see cref="HomeStepsEvents"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="HomeStepsEvents"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="HomeStepsEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static HomeStepsEventsFlags GetPayload(HarpMessage message)
        {
            return (HomeStepsEventsFlags)message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="HomeStepsEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<HomeStepsEventsFlags> GetTimestampedPayload(HarpMessage message)
        {
            var payload = message.GetTimestampedPayloadByte();
            return Timestamped.Create((HomeStepsEventsFlags)payload.Value, payload.Seconds);
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="HomeStepsEvents"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeStepsEvents"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, HomeStepsEventsFlags value)
        {
            return HarpMessage.FromByte(Address, messageType, (byte)value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="HomeStepsEvents"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeStepsEvents"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, HomeStepsEventsFlags value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, (byte)value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// HomeStepsEvents register.
    /// </summary>
    /// <seealso cref="HomeStepsEvents"/>
    [Description("Filters and selects timestamped messages from the HomeStepsEvents register.")]
    public partial class TimestampedHomeStepsEvents
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeStepsEvents"/> register. This field is constant.
        /// </summary>
        public const int Address = HomeStepsEvents.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="HomeStepsEvents"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<HomeStepsEventsFlags> GetPayload(HarpMessage message)
        {
            return HomeStepsEvents.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that sets the fixed velocity for the homing movement (steps/s).
    /// </summary>
    [Description("Sets the fixed velocity for the homing movement (steps/s)")]
    public partial class HomeVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = 50;

        /// <summary>
        /// Represents the payload type of the <see cref="HomeVelocity"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U32;

        /// <summary>
        /// Represents the length of the <see cref="HomeVelocity"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="HomeVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static uint GetPayload(HarpMessage message)
        {
            return message.GetPayloadUInt32();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="HomeVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<uint> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadUInt32();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="HomeVelocity"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeVelocity"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, uint value)
        {
            return HarpMessage.FromUInt32(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="HomeVelocity"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeVelocity"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, uint value)
        {
            return HarpMessage.FromUInt32(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// HomeVelocity register.
    /// </summary>
    /// <seealso cref="HomeVelocity"/>
    [Description("Filters and selects timestamped messages from the HomeVelocity register.")]
    public partial class TimestampedHomeVelocity
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeVelocity"/> register. This field is constant.
        /// </summary>
        public const int Address = HomeVelocity.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="HomeVelocity"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<uint> GetPayload(HarpMessage message)
        {
            return HomeVelocity.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents a register that contains the state of the home switch.
    /// </summary>
    [Description("Contains the state of the home switch.")]
    public partial class HomeSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = 51;

        /// <summary>
        /// Represents the payload type of the <see cref="HomeSwitch"/> register. This field is constant.
        /// </summary>
        public const PayloadType RegisterType = PayloadType.U8;

        /// <summary>
        /// Represents the length of the <see cref="HomeSwitch"/> register. This field is constant.
        /// </summary>
        public const int RegisterLength = 1;

        /// <summary>
        /// Returns the payload data for <see cref="HomeSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the message payload.</returns>
        public static byte GetPayload(HarpMessage message)
        {
            return message.GetPayloadByte();
        }

        /// <summary>
        /// Returns the timestamped payload data for <see cref="HomeSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetTimestampedPayload(HarpMessage message)
        {
            return message.GetTimestampedPayloadByte();
        }

        /// <summary>
        /// Returns a Harp message for the <see cref="HomeSwitch"/> register.
        /// </summary>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeSwitch"/> register
        /// with the specified message type and payload.
        /// </returns>
        public static HarpMessage FromPayload(MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, messageType, value);
        }

        /// <summary>
        /// Returns a timestamped Harp message for the <see cref="HomeSwitch"/>
        /// register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">The type of the Harp message.</param>
        /// <param name="value">The value to be stored in the message payload.</param>
        /// <returns>
        /// A <see cref="HarpMessage"/> object for the <see cref="HomeSwitch"/> register
        /// with the specified message type, timestamp, and payload.
        /// </returns>
        public static HarpMessage FromPayload(double timestamp, MessageType messageType, byte value)
        {
            return HarpMessage.FromByte(Address, timestamp, messageType, value);
        }
    }

    /// <summary>
    /// Provides methods for manipulating timestamped messages from the
    /// HomeSwitch register.
    /// </summary>
    /// <seealso cref="HomeSwitch"/>
    [Description("Filters and selects timestamped messages from the HomeSwitch register.")]
    public partial class TimestampedHomeSwitch
    {
        /// <summary>
        /// Represents the address of the <see cref="HomeSwitch"/> register. This field is constant.
        /// </summary>
        public const int Address = HomeSwitch.Address;

        /// <summary>
        /// Returns timestamped payload data for <see cref="HomeSwitch"/> register messages.
        /// </summary>
        /// <param name="message">A <see cref="HarpMessage"/> object representing the register message.</param>
        /// <returns>A value representing the timestamped message payload.</returns>
        public static Timestamped<byte> GetPayload(HarpMessage message)
        {
            return HomeSwitch.GetTimestampedPayload(message);
        }
    }

    /// <summary>
    /// Represents an operator which creates standard message payloads for the
    /// FastStepper device.
    /// </summary>
    /// <seealso cref="CreateControlPayload"/>
    /// <seealso cref="CreateEncoderPayload"/>
    /// <seealso cref="CreateAnalogInputPayload"/>
    /// <seealso cref="CreateStopSwitchPayload"/>
    /// <seealso cref="CreateMotorBrakePayload"/>
    /// <seealso cref="CreateMovingPayload"/>
    /// <seealso cref="CreateStopMovementPayload"/>
    /// <seealso cref="CreateDirectVelocityPayload"/>
    /// <seealso cref="CreateMoveToPayload"/>
    /// <seealso cref="CreateMoveToEventsPayload"/>
    /// <seealso cref="CreateMinVelocityPayload"/>
    /// <seealso cref="CreateMaxVelocityPayload"/>
    /// <seealso cref="CreateAccelerationPayload"/>
    /// <seealso cref="CreateDecelerationPayload"/>
    /// <seealso cref="CreateAccelerationJerkPayload"/>
    /// <seealso cref="CreateDecelerationJerkPayload"/>
    /// <seealso cref="CreateHomeStepsPayload"/>
    /// <seealso cref="CreateHomeStepsEventsPayload"/>
    /// <seealso cref="CreateHomeVelocityPayload"/>
    /// <seealso cref="CreateHomeSwitchPayload"/>
    [XmlInclude(typeof(CreateControlPayload))]
    [XmlInclude(typeof(CreateEncoderPayload))]
    [XmlInclude(typeof(CreateAnalogInputPayload))]
    [XmlInclude(typeof(CreateStopSwitchPayload))]
    [XmlInclude(typeof(CreateMotorBrakePayload))]
    [XmlInclude(typeof(CreateMovingPayload))]
    [XmlInclude(typeof(CreateStopMovementPayload))]
    [XmlInclude(typeof(CreateDirectVelocityPayload))]
    [XmlInclude(typeof(CreateMoveToPayload))]
    [XmlInclude(typeof(CreateMoveToEventsPayload))]
    [XmlInclude(typeof(CreateMinVelocityPayload))]
    [XmlInclude(typeof(CreateMaxVelocityPayload))]
    [XmlInclude(typeof(CreateAccelerationPayload))]
    [XmlInclude(typeof(CreateDecelerationPayload))]
    [XmlInclude(typeof(CreateAccelerationJerkPayload))]
    [XmlInclude(typeof(CreateDecelerationJerkPayload))]
    [XmlInclude(typeof(CreateHomeStepsPayload))]
    [XmlInclude(typeof(CreateHomeStepsEventsPayload))]
    [XmlInclude(typeof(CreateHomeVelocityPayload))]
    [XmlInclude(typeof(CreateHomeSwitchPayload))]
    [XmlInclude(typeof(CreateTimestampedControlPayload))]
    [XmlInclude(typeof(CreateTimestampedEncoderPayload))]
    [XmlInclude(typeof(CreateTimestampedAnalogInputPayload))]
    [XmlInclude(typeof(CreateTimestampedStopSwitchPayload))]
    [XmlInclude(typeof(CreateTimestampedMotorBrakePayload))]
    [XmlInclude(typeof(CreateTimestampedMovingPayload))]
    [XmlInclude(typeof(CreateTimestampedStopMovementPayload))]
    [XmlInclude(typeof(CreateTimestampedDirectVelocityPayload))]
    [XmlInclude(typeof(CreateTimestampedMoveToPayload))]
    [XmlInclude(typeof(CreateTimestampedMoveToEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedMinVelocityPayload))]
    [XmlInclude(typeof(CreateTimestampedMaxVelocityPayload))]
    [XmlInclude(typeof(CreateTimestampedAccelerationPayload))]
    [XmlInclude(typeof(CreateTimestampedDecelerationPayload))]
    [XmlInclude(typeof(CreateTimestampedAccelerationJerkPayload))]
    [XmlInclude(typeof(CreateTimestampedDecelerationJerkPayload))]
    [XmlInclude(typeof(CreateTimestampedHomeStepsPayload))]
    [XmlInclude(typeof(CreateTimestampedHomeStepsEventsPayload))]
    [XmlInclude(typeof(CreateTimestampedHomeVelocityPayload))]
    [XmlInclude(typeof(CreateTimestampedHomeSwitchPayload))]
    [Description("Creates standard message payloads for the FastStepper device.")]
    public partial class CreateMessage : CreateMessageBuilder, INamedElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateMessage"/> class.
        /// </summary>
        public CreateMessage()
        {
            Payload = new CreateControlPayload();
        }

        string INamedElement.Name => $"{nameof(FastStepper)}.{GetElementDisplayName(Payload)}";
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that control the device modules.
    /// </summary>
    [DisplayName("ControlPayload")]
    [Description("Creates a message payload that control the device modules.")]
    public partial class CreateControlPayload
    {
        /// <summary>
        /// Gets or sets the value that control the device modules.
        /// </summary>
        [Description("The value that control the device modules.")]
        public ControlFlags Control { get; set; }

        /// <summary>
        /// Creates a message payload for the Control register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ControlFlags GetPayload()
        {
            return Control;
        }

        /// <summary>
        /// Creates a message that control the device modules.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Control register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Control.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that control the device modules.
    /// </summary>
    [DisplayName("TimestampedControlPayload")]
    [Description("Creates a timestamped message payload that control the device modules.")]
    public partial class CreateTimestampedControlPayload : CreateControlPayload
    {
        /// <summary>
        /// Creates a timestamped message that control the device modules.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Control register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Control.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the reading of the quadrature encoder.
    /// </summary>
    [DisplayName("EncoderPayload")]
    [Description("Creates a message payload that contains the reading of the quadrature encoder.")]
    public partial class CreateEncoderPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the reading of the quadrature encoder.
        /// </summary>
        [Description("The value that contains the reading of the quadrature encoder.")]
        public short Encoder { get; set; }

        /// <summary>
        /// Creates a message payload for the Encoder register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public short GetPayload()
        {
            return Encoder;
        }

        /// <summary>
        /// Creates a message that contains the reading of the quadrature encoder.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Encoder register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Encoder.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the reading of the quadrature encoder.
    /// </summary>
    [DisplayName("TimestampedEncoderPayload")]
    [Description("Creates a timestamped message payload that contains the reading of the quadrature encoder.")]
    public partial class CreateTimestampedEncoderPayload : CreateEncoderPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the reading of the quadrature encoder.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Encoder register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Encoder.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the reading of the analog input.
    /// </summary>
    [DisplayName("AnalogInputPayload")]
    [Description("Creates a message payload that contains the reading of the analog input.")]
    public partial class CreateAnalogInputPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the reading of the analog input.
        /// </summary>
        [Description("The value that contains the reading of the analog input.")]
        public short AnalogInput { get; set; }

        /// <summary>
        /// Creates a message payload for the AnalogInput register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public short GetPayload()
        {
            return AnalogInput;
        }

        /// <summary>
        /// Creates a message that contains the reading of the analog input.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AnalogInput register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.AnalogInput.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the reading of the analog input.
    /// </summary>
    [DisplayName("TimestampedAnalogInputPayload")]
    [Description("Creates a timestamped message payload that contains the reading of the analog input.")]
    public partial class CreateTimestampedAnalogInputPayload : CreateAnalogInputPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the reading of the analog input.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AnalogInput register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.AnalogInput.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the state of the stop switch.
    /// </summary>
    [DisplayName("StopSwitchPayload")]
    [Description("Creates a message payload that contains the state of the stop switch.")]
    public partial class CreateStopSwitchPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the state of the stop switch.
        /// </summary>
        [Description("The value that contains the state of the stop switch.")]
        public StopSwitchFlags StopSwitch { get; set; }

        /// <summary>
        /// Creates a message payload for the StopSwitch register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public StopSwitchFlags GetPayload()
        {
            return StopSwitch;
        }

        /// <summary>
        /// Creates a message that contains the state of the stop switch.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StopSwitch register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.StopSwitch.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the state of the stop switch.
    /// </summary>
    [DisplayName("TimestampedStopSwitchPayload")]
    [Description("Creates a timestamped message payload that contains the state of the stop switch.")]
    public partial class CreateTimestampedStopSwitchPayload : CreateStopSwitchPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the state of the stop switch.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StopSwitch register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.StopSwitch.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the state of the motor brake output.
    /// </summary>
    [DisplayName("MotorBrakePayload")]
    [Description("Creates a message payload that sets the state of the motor brake output.")]
    public partial class CreateMotorBrakePayload
    {
        /// <summary>
        /// Gets or sets the value that sets the state of the motor brake output.
        /// </summary>
        [Description("The value that sets the state of the motor brake output.")]
        public MotorBrakeFlags MotorBrake { get; set; }

        /// <summary>
        /// Creates a message payload for the MotorBrake register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MotorBrakeFlags GetPayload()
        {
            return MotorBrake;
        }

        /// <summary>
        /// Creates a message that sets the state of the motor brake output.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MotorBrake register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.MotorBrake.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the state of the motor brake output.
    /// </summary>
    [DisplayName("TimestampedMotorBrakePayload")]
    [Description("Creates a timestamped message payload that sets the state of the motor brake output.")]
    public partial class CreateTimestampedMotorBrakePayload : CreateMotorBrakePayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the state of the motor brake output.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MotorBrake register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.MotorBrake.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the state of the motor movement.
    /// </summary>
    [DisplayName("MovingPayload")]
    [Description("Creates a message payload that contains the state of the motor movement.")]
    public partial class CreateMovingPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the state of the motor movement.
        /// </summary>
        [Description("The value that contains the state of the motor movement.")]
        public MovingFlags Moving { get; set; }

        /// <summary>
        /// Creates a message payload for the Moving register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public MovingFlags GetPayload()
        {
            return Moving;
        }

        /// <summary>
        /// Creates a message that contains the state of the motor movement.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Moving register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Moving.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the state of the motor movement.
    /// </summary>
    [DisplayName("TimestampedMovingPayload")]
    [Description("Creates a timestamped message payload that contains the state of the motor movement.")]
    public partial class CreateTimestampedMovingPayload : CreateMovingPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the state of the motor movement.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Moving register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Moving.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that instantly stops the motor movement.
    /// </summary>
    [DisplayName("StopMovementPayload")]
    [Description("Creates a message payload that instantly stops the motor movement.")]
    public partial class CreateStopMovementPayload
    {
        /// <summary>
        /// Gets or sets the value that instantly stops the motor movement.
        /// </summary>
        [Description("The value that instantly stops the motor movement.")]
        public byte StopMovement { get; set; }

        /// <summary>
        /// Creates a message payload for the StopMovement register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return StopMovement;
        }

        /// <summary>
        /// Creates a message that instantly stops the motor movement.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the StopMovement register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.StopMovement.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that instantly stops the motor movement.
    /// </summary>
    [DisplayName("TimestampedStopMovementPayload")]
    [Description("Creates a timestamped message payload that instantly stops the motor movement.")]
    public partial class CreateTimestampedStopMovementPayload : CreateStopMovementPayload
    {
        /// <summary>
        /// Creates a timestamped message that instantly stops the motor movement.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the StopMovement register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.StopMovement.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that instantly start moving at a specific speed and direction according to the register's value and signal.
    /// </summary>
    [DisplayName("DirectVelocityPayload")]
    [Description("Creates a message payload that instantly start moving at a specific speed and direction according to the register's value and signal.")]
    public partial class CreateDirectVelocityPayload
    {
        /// <summary>
        /// Gets or sets the value that instantly start moving at a specific speed and direction according to the register's value and signal.
        /// </summary>
        [Description("The value that instantly start moving at a specific speed and direction according to the register's value and signal.")]
        public int DirectVelocity { get; set; }

        /// <summary>
        /// Creates a message payload for the DirectVelocity register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return DirectVelocity;
        }

        /// <summary>
        /// Creates a message that instantly start moving at a specific speed and direction according to the register's value and signal.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DirectVelocity register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.DirectVelocity.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that instantly start moving at a specific speed and direction according to the register's value and signal.
    /// </summary>
    [DisplayName("TimestampedDirectVelocityPayload")]
    [Description("Creates a timestamped message payload that instantly start moving at a specific speed and direction according to the register's value and signal.")]
    public partial class CreateTimestampedDirectVelocityPayload : CreateDirectVelocityPayload
    {
        /// <summary>
        /// Creates a timestamped message that instantly start moving at a specific speed and direction according to the register's value and signal.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DirectVelocity register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.DirectVelocity.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that moves to a specific position, using the velocity, acceleration and jerk configurations.
    /// </summary>
    [DisplayName("MoveToPayload")]
    [Description("Creates a message payload that moves to a specific position, using the velocity, acceleration and jerk configurations.")]
    public partial class CreateMoveToPayload
    {
        /// <summary>
        /// Gets or sets the value that moves to a specific position, using the velocity, acceleration and jerk configurations.
        /// </summary>
        [Description("The value that moves to a specific position, using the velocity, acceleration and jerk configurations.")]
        public int MoveTo { get; set; }

        /// <summary>
        /// Creates a message payload for the MoveTo register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return MoveTo;
        }

        /// <summary>
        /// Creates a message that moves to a specific position, using the velocity, acceleration and jerk configurations.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MoveTo register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.MoveTo.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that moves to a specific position, using the velocity, acceleration and jerk configurations.
    /// </summary>
    [DisplayName("TimestampedMoveToPayload")]
    [Description("Creates a timestamped message payload that moves to a specific position, using the velocity, acceleration and jerk configurations.")]
    public partial class CreateTimestampedMoveToPayload : CreateMoveToPayload
    {
        /// <summary>
        /// Creates a timestamped message that moves to a specific position, using the velocity, acceleration and jerk configurations.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MoveTo register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.MoveTo.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
    /// </summary>
    [DisplayName("MoveToEventsPayload")]
    [Description("Creates a message payload that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.")]
    public partial class CreateMoveToEventsPayload
    {
        /// <summary>
        /// Gets or sets the value that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
        /// </summary>
        [Description("The value that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.")]
        public byte MoveToEvents { get; set; }

        /// <summary>
        /// Creates a message payload for the MoveToEvents register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return MoveToEvents;
        }

        /// <summary>
        /// Creates a message that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MoveToEvents register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.MoveToEvents.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
    /// </summary>
    [DisplayName("TimestampedMoveToEventsPayload")]
    [Description("Creates a timestamped message payload that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.")]
    public partial class CreateTimestampedMoveToEventsPayload : CreateMoveToEventsPayload
    {
        /// <summary>
        /// Creates a timestamped message that reports possible events regarding the execution of the ADD_REG_MOVE_TO register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MoveToEvents register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.MoveToEvents.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the minimum velocity for the movement (steps/s).
    /// </summary>
    [DisplayName("MinVelocityPayload")]
    [Description("Creates a message payload that sets the minimum velocity for the movement (steps/s).")]
    public partial class CreateMinVelocityPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the minimum velocity for the movement (steps/s).
        /// </summary>
        [Description("The value that sets the minimum velocity for the movement (steps/s).")]
        public ushort MinVelocity { get; set; }

        /// <summary>
        /// Creates a message payload for the MinVelocity register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return MinVelocity;
        }

        /// <summary>
        /// Creates a message that sets the minimum velocity for the movement (steps/s).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MinVelocity register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.MinVelocity.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the minimum velocity for the movement (steps/s).
    /// </summary>
    [DisplayName("TimestampedMinVelocityPayload")]
    [Description("Creates a timestamped message payload that sets the minimum velocity for the movement (steps/s).")]
    public partial class CreateTimestampedMinVelocityPayload : CreateMinVelocityPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the minimum velocity for the movement (steps/s).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MinVelocity register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.MinVelocity.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the maximum velocity for the movement (steps/s).
    /// </summary>
    [DisplayName("MaxVelocityPayload")]
    [Description("Creates a message payload that sets the maximum velocity for the movement (steps/s).")]
    public partial class CreateMaxVelocityPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the maximum velocity for the movement (steps/s).
        /// </summary>
        [Description("The value that sets the maximum velocity for the movement (steps/s).")]
        public ushort MaxVelocity { get; set; }

        /// <summary>
        /// Creates a message payload for the MaxVelocity register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public ushort GetPayload()
        {
            return MaxVelocity;
        }

        /// <summary>
        /// Creates a message that sets the maximum velocity for the movement (steps/s).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the MaxVelocity register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.MaxVelocity.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the maximum velocity for the movement (steps/s).
    /// </summary>
    [DisplayName("TimestampedMaxVelocityPayload")]
    [Description("Creates a timestamped message payload that sets the maximum velocity for the movement (steps/s).")]
    public partial class CreateTimestampedMaxVelocityPayload : CreateMaxVelocityPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the maximum velocity for the movement (steps/s).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the MaxVelocity register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.MaxVelocity.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the acceleration for the movement (steps/s^2).
    /// </summary>
    [DisplayName("AccelerationPayload")]
    [Description("Creates a message payload that sets the acceleration for the movement (steps/s^2).")]
    public partial class CreateAccelerationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the acceleration for the movement (steps/s^2).
        /// </summary>
        [Description("The value that sets the acceleration for the movement (steps/s^2).")]
        public int Acceleration { get; set; }

        /// <summary>
        /// Creates a message payload for the Acceleration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return Acceleration;
        }

        /// <summary>
        /// Creates a message that sets the acceleration for the movement (steps/s^2).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Acceleration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Acceleration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the acceleration for the movement (steps/s^2).
    /// </summary>
    [DisplayName("TimestampedAccelerationPayload")]
    [Description("Creates a timestamped message payload that sets the acceleration for the movement (steps/s^2).")]
    public partial class CreateTimestampedAccelerationPayload : CreateAccelerationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the acceleration for the movement (steps/s^2).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Acceleration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Acceleration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the deceleration for the movement (steps/s^2).
    /// </summary>
    [DisplayName("DecelerationPayload")]
    [Description("Creates a message payload that sets the deceleration for the movement (steps/s^2).")]
    public partial class CreateDecelerationPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the deceleration for the movement (steps/s^2).
        /// </summary>
        [Description("The value that sets the deceleration for the movement (steps/s^2).")]
        public int Deceleration { get; set; }

        /// <summary>
        /// Creates a message payload for the Deceleration register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return Deceleration;
        }

        /// <summary>
        /// Creates a message that sets the deceleration for the movement (steps/s^2).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the Deceleration register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.Deceleration.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the deceleration for the movement (steps/s^2).
    /// </summary>
    [DisplayName("TimestampedDecelerationPayload")]
    [Description("Creates a timestamped message payload that sets the deceleration for the movement (steps/s^2).")]
    public partial class CreateTimestampedDecelerationPayload : CreateDecelerationPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the deceleration for the movement (steps/s^2).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the Deceleration register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.Deceleration.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the jerk for the acceleration part of the movement (steps/s^3).
    /// </summary>
    [DisplayName("AccelerationJerkPayload")]
    [Description("Creates a message payload that sets the jerk for the acceleration part of the movement (steps/s^3).")]
    public partial class CreateAccelerationJerkPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the jerk for the acceleration part of the movement (steps/s^3).
        /// </summary>
        [Description("The value that sets the jerk for the acceleration part of the movement (steps/s^3).")]
        public int AccelerationJerk { get; set; }

        /// <summary>
        /// Creates a message payload for the AccelerationJerk register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return AccelerationJerk;
        }

        /// <summary>
        /// Creates a message that sets the jerk for the acceleration part of the movement (steps/s^3).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the AccelerationJerk register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.AccelerationJerk.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the jerk for the acceleration part of the movement (steps/s^3).
    /// </summary>
    [DisplayName("TimestampedAccelerationJerkPayload")]
    [Description("Creates a timestamped message payload that sets the jerk for the acceleration part of the movement (steps/s^3).")]
    public partial class CreateTimestampedAccelerationJerkPayload : CreateAccelerationJerkPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the jerk for the acceleration part of the movement (steps/s^3).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the AccelerationJerk register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.AccelerationJerk.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the jerk for the deceleration part of the movement (steps/s^3).
    /// </summary>
    [DisplayName("DecelerationJerkPayload")]
    [Description("Creates a message payload that sets the jerk for the deceleration part of the movement (steps/s^3).")]
    public partial class CreateDecelerationJerkPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the jerk for the deceleration part of the movement (steps/s^3).
        /// </summary>
        [Description("The value that sets the jerk for the deceleration part of the movement (steps/s^3).")]
        public int DecelerationJerk { get; set; }

        /// <summary>
        /// Creates a message payload for the DecelerationJerk register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return DecelerationJerk;
        }

        /// <summary>
        /// Creates a message that sets the jerk for the deceleration part of the movement (steps/s^3).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the DecelerationJerk register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.DecelerationJerk.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the jerk for the deceleration part of the movement (steps/s^3).
    /// </summary>
    [DisplayName("TimestampedDecelerationJerkPayload")]
    [Description("Creates a timestamped message payload that sets the jerk for the deceleration part of the movement (steps/s^3).")]
    public partial class CreateTimestampedDecelerationJerkPayload : CreateDecelerationJerkPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the jerk for the deceleration part of the movement (steps/s^3).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the DecelerationJerk register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.DecelerationJerk.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
    /// </summary>
    [DisplayName("HomeStepsPayload")]
    [Description("Creates a message payload that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.")]
    public partial class CreateHomeStepsPayload
    {
        /// <summary>
        /// Gets or sets the value that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
        /// </summary>
        [Description("The value that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.")]
        public int HomeSteps { get; set; }

        /// <summary>
        /// Creates a message payload for the HomeSteps register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public int GetPayload()
        {
            return HomeSteps;
        }

        /// <summary>
        /// Creates a message that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the HomeSteps register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.HomeSteps.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
    /// </summary>
    [DisplayName("TimestampedHomeStepsPayload")]
    [Description("Creates a timestamped message payload that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.")]
    public partial class CreateTimestampedHomeStepsPayload : CreateHomeStepsPayload
    {
        /// <summary>
        /// Creates a timestamped message that moves a specific number of steps in a direction according to the register's value and signal, attempting to perform a homing routine. Resets the current position to 0 when the home sensor is hit. The home steps value should be slightly over than the longest possible movement.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the HomeSteps register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.HomeSteps.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that reports possible events regarding the execution of the REG_HOME_STEPS register.
    /// </summary>
    [DisplayName("HomeStepsEventsPayload")]
    [Description("Creates a message payload that reports possible events regarding the execution of the REG_HOME_STEPS register.")]
    public partial class CreateHomeStepsEventsPayload
    {
        /// <summary>
        /// Gets or sets the value that reports possible events regarding the execution of the REG_HOME_STEPS register.
        /// </summary>
        [Description("The value that reports possible events regarding the execution of the REG_HOME_STEPS register.")]
        public HomeStepsEventsFlags HomeStepsEvents { get; set; }

        /// <summary>
        /// Creates a message payload for the HomeStepsEvents register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public HomeStepsEventsFlags GetPayload()
        {
            return HomeStepsEvents;
        }

        /// <summary>
        /// Creates a message that reports possible events regarding the execution of the REG_HOME_STEPS register.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the HomeStepsEvents register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.HomeStepsEvents.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that reports possible events regarding the execution of the REG_HOME_STEPS register.
    /// </summary>
    [DisplayName("TimestampedHomeStepsEventsPayload")]
    [Description("Creates a timestamped message payload that reports possible events regarding the execution of the REG_HOME_STEPS register.")]
    public partial class CreateTimestampedHomeStepsEventsPayload : CreateHomeStepsEventsPayload
    {
        /// <summary>
        /// Creates a timestamped message that reports possible events regarding the execution of the REG_HOME_STEPS register.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the HomeStepsEvents register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.HomeStepsEvents.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that sets the fixed velocity for the homing movement (steps/s).
    /// </summary>
    [DisplayName("HomeVelocityPayload")]
    [Description("Creates a message payload that sets the fixed velocity for the homing movement (steps/s).")]
    public partial class CreateHomeVelocityPayload
    {
        /// <summary>
        /// Gets or sets the value that sets the fixed velocity for the homing movement (steps/s).
        /// </summary>
        [Description("The value that sets the fixed velocity for the homing movement (steps/s).")]
        public uint HomeVelocity { get; set; }

        /// <summary>
        /// Creates a message payload for the HomeVelocity register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public uint GetPayload()
        {
            return HomeVelocity;
        }

        /// <summary>
        /// Creates a message that sets the fixed velocity for the homing movement (steps/s).
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the HomeVelocity register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.HomeVelocity.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that sets the fixed velocity for the homing movement (steps/s).
    /// </summary>
    [DisplayName("TimestampedHomeVelocityPayload")]
    [Description("Creates a timestamped message payload that sets the fixed velocity for the homing movement (steps/s).")]
    public partial class CreateTimestampedHomeVelocityPayload : CreateHomeVelocityPayload
    {
        /// <summary>
        /// Creates a timestamped message that sets the fixed velocity for the homing movement (steps/s).
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the HomeVelocity register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.HomeVelocity.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a message payload
    /// that contains the state of the home switch.
    /// </summary>
    [DisplayName("HomeSwitchPayload")]
    [Description("Creates a message payload that contains the state of the home switch.")]
    public partial class CreateHomeSwitchPayload
    {
        /// <summary>
        /// Gets or sets the value that contains the state of the home switch.
        /// </summary>
        [Description("The value that contains the state of the home switch.")]
        public byte HomeSwitch { get; set; }

        /// <summary>
        /// Creates a message payload for the HomeSwitch register.
        /// </summary>
        /// <returns>The created message payload value.</returns>
        public byte GetPayload()
        {
            return HomeSwitch;
        }

        /// <summary>
        /// Creates a message that contains the state of the home switch.
        /// </summary>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new message for the HomeSwitch register.</returns>
        public HarpMessage GetMessage(MessageType messageType)
        {
            return Harp.FastStepper.HomeSwitch.FromPayload(messageType, GetPayload());
        }
    }

    /// <summary>
    /// Represents an operator that creates a timestamped message payload
    /// that contains the state of the home switch.
    /// </summary>
    [DisplayName("TimestampedHomeSwitchPayload")]
    [Description("Creates a timestamped message payload that contains the state of the home switch.")]
    public partial class CreateTimestampedHomeSwitchPayload : CreateHomeSwitchPayload
    {
        /// <summary>
        /// Creates a timestamped message that contains the state of the home switch.
        /// </summary>
        /// <param name="timestamp">The timestamp of the message payload, in seconds.</param>
        /// <param name="messageType">Specifies the type of the created message.</param>
        /// <returns>A new timestamped message for the HomeSwitch register.</returns>
        public HarpMessage GetMessage(double timestamp, MessageType messageType)
        {
            return Harp.FastStepper.HomeSwitch.FromPayload(timestamp, messageType, GetPayload());
        }
    }

    /// <summary>
    /// Available device module configuration flags.
    /// </summary>
    [Flags]
    public enum ControlFlags : ushort
    {
        None = 0x0,
        EnableMotor = 0x1,
        DisableMotor = 0x2,
        EnableAnalogInput = 0x4,
        DisableAnalogInput = 0x8,
        EnableEncoder = 0x10,
        DisableEncoder = 0x20,
        ResetEncoder = 0x40,
        EnableHoming = 0x80,
        DisableHoming = 0x100
    }

    /// <summary>
    /// Flags describing the state of the motor stop switch.
    /// </summary>
    [Flags]
    public enum StopSwitchFlags : byte
    {
        None = 0x0,
        StopSwitch = 0x1
    }

    /// <summary>
    /// Flags describing the movement state of the motor.
    /// </summary>
    [Flags]
    public enum MovingFlags : byte
    {
        None = 0x0,
        IsMoving = 0x1
    }

    /// <summary>
    /// Flags describing the state of the motor stop switch.
    /// </summary>
    [Flags]
    public enum HomeSwitchFlags : byte
    {
        None = 0x0,
        HomeSwitch = 0x1
    }

    /// <summary>
    /// Flags describing the state of the motor stop switch.
    /// </summary>
    [Flags]
    public enum MotorBrakeFlags : byte
    {
        None = 0x0,
        HomeSwitch = 0x1
    }

    /// <summary>
    /// Flags describing the status of the homing sequence.
    /// </summary>
    [Flags]
    public enum HomeStepsEventsFlags : byte
    {
        None = 0x0,
        HomingSuccessful = 0x1,
        HomingFailed = 0x2,
        AlreadyHome = 0x4,
        UnexpectedHome = 0x8,
        HomingDisabled = 0x10,
        HomingMissing = 0x20
    }
}
