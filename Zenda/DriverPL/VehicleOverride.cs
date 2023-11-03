using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zenda.DriverPL
{
    public class VehicleOverride
    {
        // Current file
        FileStream file;
        VehicleOverrideFile vehicleOverride;

        BinaryReader binaryReader;

        // Get binary HUD from given file name
        public VehicleOverride(FileStream filed)
        {
            file = filed;

            vehicleOverride.vID = binaryReader.ReadInt32();
            vehicleOverride.magic = binaryReader.ReadBytes(4);
            vehicleOverride.version = binaryReader.ReadInt32();
            vehicleOverride.amount = binaryReader.ReadInt32();

            VehicleOverrideParameterSchemeTypes vehicleOverrideParameterSchemeTypes = new VehicleOverrideParameterSchemeTypes();

            for (int i = 1; i <= vehicleOverride.amount; i++)
            {
                VehicleOverrideParameter parameter = new VehicleOverrideParameter();

                UInt16 pID = binaryReader.ReadUInt16();

                binaryReader.ReadBytes(2);

                string typeDefine = null;

                // Look through the dictionary to find if the pID has a float, int or bool typeValue
                if (vehicleOverrideParameterSchemeTypes.DriverPL.TryGetValue(pID, out string typeInfo))
                {
                    typeDefine = typeInfo;
                }

                byte[] dValue = binaryReader.ReadBytes(4); // Old decimal-based hex value
                //string cValue = string.Join("", dValue.Select(b => b.ToString("X2"))); // New heximal-based hex value

                parameter.pID = pID;
                parameter.type = typeDefine;
                parameter.value = dValue;

                vehicleOverride.vehicleOverrideParameters.Add(parameter);
            }

            vehicleOverride.terminator = binaryReader.ReadBytes(4);
        }

        public VehicleOverrideFile getVehicleOverride()
        {
            return vehicleOverride;
        }
    }

    public class VehicleOverrideFile
    {
        public int vID;
        public byte[] magic;
        public int version;
        public int amount;

        public List<VehicleOverrideParameter> vehicleOverrideParameters;

        public byte[] terminator;
    }

    public class VehicleOverrideParameter
    {
        public int pID;
        public string type;
        public byte[] value;
    }

    public class VehicleOverrideParameterSchemes
    {
        public Dictionary<UInt16, string> DriverPL = new Dictionary<UInt16, string>
        {
            {  0,  "Mass" },
            {  1,  "Suspension Force" },
            {  2,  "Suspension Damping" },
            {  3,  "Maximum Suspension Compression" },
            {  4,  "Suspension Normal" },
            {  5,  "Suspension Initial" },
            {  6,  "Friction K0" },
            {  7,  "Friction K1" },
            {  8,  "Friction K2" },
            {  9,  "Flfriction" },
            {  10, "Friction Base" },
            {  11, "Ffast" },
            {  12, "Rfast" },
            {  13, "Rolling Friction" },
            {  14, "Angular Damping" },
            {  15, "Motion X" },
            {  16, "Motion Y" },
            {  17, "Motion Z" },
            {  18, "Thrust Forward" },
            {  19, "Thrust Reverse" },
            {  20, "Thrust Spin" },
            {  21, "Wheelspin Threshold" },
            {  22, "Reverse Threshold" },
            {  23, "Reverse Scale" },
            {  24, "Footbrake Unlock" },
            {  25, "Ride Height" },
            {  26, "Motion Scale" },
            {  27, "Rear Lat Friction" },
            {  28, "Motion Scale 2" },
            {  29, "Allow Auto Brake" },
            {  30, "Center Of Gravity Height" },
            {  31, "Center Of Mass Offset Y" },
            {  32, "Center Of Mass Offset Z" },
            {  33, "Primary Axis Rotate X" },
            {  34, "Primary Axis Rotate Y" },
            {  35, "Primary Axis Rotate Z" },
            {  36, "Toughness" },
            {  37, "Fragility" },
            {  38, "Front Crumple Length" },
            {  39, "Rear Crumple Length" },
            {  40, "Rpm 0" },
            {  41, "Rpm 1" },
            {  42, "Rpm 2" },
            {  43, "Impulse Scale" },
            {  44, "Rev Up" },
            {  45, "Rev Up 2" },
            {  46, "Rev Down" },
            {  47, "Rev Down 2" },
            {  48, "Ratio 0" },
            {  49, "Ratio 1" },
            {  50, "Ratio 2" },
            {  51, "Ratio 3" },
            {  52, "Ratio 4" },
            {  53, "Ratio 5" },
            {  54, "Ratio 6" },
            {  55, "Bike Crash Lat" },
            {  56, "Bike Crash Norm" },
            {  57, "ASCH LF" },
            {  58, "ASCH FS" },
            {  59, "Handbrake Spin" },
            {  60, "Engine Heat" },
            {  61, "Engine Cool" },
            {  62, "Boat On Throttle Flick Reduce" },
            {  63, "Boat Off Throttle Flick Reduce" },
            {  64, "Boat Plane Speed 0" },
            {  65, "Boat Plane Speed 1" },
            {  66, "Boat Plane Speed 2" },
            {  67, "Height" },
            {  68, "Width" },
            {  69, "Length" },
            {  70, "Lat Grip" },
            {  71, "Lat Grip Clip" },
            {  72, "Front Brake" },
            {  73, "Rear Brake" },
            {  74, "Full Steer Speed" },
            {  75, "Steer Twist" },
            {  76, "Wheelie Steer Factor" },
            {  77, "Brake Better Steering" },
            {  78, "Hand Rear Lat" },
            {  79, "Hand Lat Front Max" },
            {  80, "Hand Lat Front Min" },
            {  81, "Hand Stop" },
            {  82, "Hand Free Stop" },
            {  83, "Hand Damp Yaw Max" },
            {  84, "Hand Damp Yaw Min" },
            {  85, "Hand Damp Yaw Rate" },
            {  86, "Burn Out Speed" },
            {  87, "Burn Out Full Lean Speed" },
            {  88, "Burn Out Damp Yaw" },
            {  89, "Burn Out Rear Lat" },
            {  90, "Burn Out Steer Long Factor" },
            {  91, "Burn Out No Steer Damp Yaw" },
            {  92, "Burn Out Counter Steer Damp Yaw" },
            {  93, "Donut Twist" },
            {  94, "Donut Long" },
            {  95, "Reverse Impulse" },
            {  96, "Rolling Resistance" },
            {  97, "Wind Resistance" },
            {  98, "Weight Transfer" },
            {  99, "Wheelie Acc Clip" },
            { 100, "Wheelie Twist" },
            { 101, "Wheelie Height" },
            { 102, "Wheelie Balance" },
            { 103, "Wheelie Stop" },
            { 104, "Stoppie Twist" },
            { 105, "Stoppie Acc Clip" },
            { 106, "Stoppie Speed" },
            { 107, "Stoppie Balance" },
            { 108, "Lean Angle" },
            { 109, "Lean Less" },
            { 110, "Lean Wheelie Factor" },
            { 111, "Full Lean Speed" },
            { 112, "Lean Twist" },
            { 113, "Maximum Lean Diff Initial" },
            { 114, "Lean Diff Rate" },
            { 115, "Maximum Lean Diff Max" },
            { 116, "Power Slide Threshold" },
            { 117, "Power Slide Lean Factor" },
            { 118, "Power Slide Lat Front" },
            { 119, "Power Slide Lat Rear" },
            { 120, "Damp Pitch" },
            { 121, "Damp Roll" },
            { 122, "Damp Yaw" },
            { 123, "Damp Yaw Rate" },
            { 124, "Yaw Clip" },
            { 125, "Damp Ang Vel In Air" },
            { 126, "Damp Ang Vel Crash" },
            { 127, "Damp Linear Vel In Air" },
            { 128, "Bike Crash Long" },
            { 129, "Regain Grip Rate" },
            { 130, "In Air On Ground" },
            { 131, "Bike Spare 1" },
            { 132, "Bike Spare 2" },
            { 133, "Bike Spare 3" },
            { 134, "Bike Spare 4" },
            { 135, "Low Speed Wheel Ang Div" },
            { 136, "High Speed Wheel Ang Div" },
            { 137, "Speed Wheel Ang" },
            { 138, "Shock Transmission" },
            { 139, "Softness" },
            { 140, "Land Gravity" },
            { 141, "Air Gravity" },
            { 142, "Colour 1 RGBA" },
            { 143, "Colour 2 RGBA" },
            { 144, "Colour 3 RGBA" },
            { 145, "Colour 4 RGBA" },
            { 146, "Colour 5 RGBA" },
            { 147, "Colour 6 RGBA" },
            { 148, "Colour 7 RGBA" },
            { 149, "Colour 8 RGBA" },
            { 150, "Colour 9 RGBA" },
            { 151, "Colour 10 RGBA" },
            { 152, "Colour 11 RGBA" },
            { 153, "Colour 12 RGBA" },
            { 154, "Colour 13 RGBA" },
            { 155, "Colour 14 RGBA" },
            { 156, "Colour 15 RGBA" },
            { 157, "Colour 16 RGBA" },
            { 158, "Front Wheel Crumple Length" },
            { 159, "Rear Wheel Crumple Length" },
            { 160, "Top Speed" },
            { 161, "WBlur 1" }, // ? Weather blur 1 ? //
            { 162, "WBlur 2" }, // ? Weather blur 2 ? //
            { 163, "WBlur 3" }, // ? Weather blur 3 ? //
            { 164, "Traffic Spawn Chance" },
            { 165, "Engine Damage Weight" },
            { 166, "Aggressive Braking Pressure" },
            { 167, "Boat Buoyancy" },
            { 168, "Boat Buoyancy Damp" },
            { 169, "Boat Forward Thrust" },
            { 170, "Boat Reverse Thrust" },
            { 171, "Boat Hull Radius" },
            { 172, "Boat Wave Scale" },
            { 173, "Boat Max Steer Angle" },
            { 174, "Boat Lean Angle" },
            { 175, "Boat Lean Twist" },
            { 176, "Boat Damp Pitch" },
            { 177, "Boat Damp Yaw" },
            { 178, "Boat Damp Yaw Huge" },
            { 179, "Boat Damp Roll" },
            { 180, "Boat Damp Up Vel" },
            { 181, "Boat Damp Forward Vel" },
            { 182, "Boat Damp Reverse Vel" },
            { 183, "Boat Damp Lateral Vel" },
            { 184, "Change Down Factor" },
            { 185, "Kick Down Factor" },
            { 186, "Blip Factor" },
            { 187, "Reverse Top Speed" },
            { 188, "Suspension Draw Clamp" },
            { 189, "Wheel Soft" },
            { 190, "Steer Angle" },
            { 191, "Siren Colour 1 RGBA" }, // int
            { 192, "Siren Colour 2 RGBA" }, // int
            { 193, "Siren Colour 3 RGBA" }, // int
            { 194, "Siren Colour 4 RGBA" }, // int
            { 195, "Vehicle Autobrakes But Does Not Steer" }, // bool
            { 196, "Vehicle Does Not Crumple" }, // bool
            { 197, "Vehicle Cannot Disintegrate" }, // bool
            { 198, "Vehicle Chassis Does Not Break" }, // bool
            { 199, "Vehicle Swaps Main Col Shapes" }, // bool
            { 200, "Vehicle Wheel Transforms Parent Assumed Identity" }, // bool
            { 201, "Vehicle Is A Cop" }, // bool
            { 202, "Vehicle Cannot Burnout" }, // bool
            { 203, "Vehicle Can Have 3D Collision Debris" }, // bool
            { 204, "Vehicle Wheels Can Detach From Impacts" }, // bool
            { 205, "Vehicle Breaks Down When Wheel Lost" }, // bool
            { 206, "Vehicle Can Go Critical" }, // bool
            { 207, "Vehicle Has Wallcrawling Problems" }, // bool
            { 208, "Vehicle Bulletproofness" },
            { 209, "Vehicle Should Always Have Full Collision" }, // bool
            { 210, "Vehicle Never Parks" }, // bool
            { 211, "Vehicle Is Jittery Insomniac" }, // bool
            { 212, "Vehicle Has Bizarre Underground Wheel Problem" }, // bool
            { 213, "Vehicle Overloads Burnout Control" }, // bool
            { 214, "Vehicle Badly Designed For Shooting Bodge Radius" }, // bool
            { 215, "Vehicle Has A Pickup Gate Boot" }, // bool
            { 216, "Vehicle Lamps Are Highly Morph Sensitive" }, // bool
            { 217, "Vehicle BBox May Be From The Wrong Collision Shape" }, // bool
            { 218, "Vehicle Has 2-in-1 Siren Lights" }, // bool
            { 219, "Vehicle Wheels Are Undetachable" }, // bool
            { 220, "Vehicle Is Unhookable Trailer" }, // bool
            { 221, "Braking Factor" },
            { 222, "Autobrake Scaler" },
            { 223, "Suspension Force Rear" },
            { 224, "Suspension Damping Rear" },
            { 225, "Ride Height Rear" },
            { 226, "Braking Bias" },
            { 227, "LOD1 Switch PS2" },
            { 228, "LOD2 Switch PS2" },
            { 229, "LOD3 Switch PS2" },
            { 230, "LOD4 Switch PS2" },
            { 231, "LOD1 Switch XB" },
            { 232, "LOD2 Switch XB" },
            { 233, "LOD3 Switch XB" },
            { 234, "LOD4 Switch XB" },
            { 235, "LOD1 Switch PC" },
            { 236, "LOD2 Switch PC" },
            { 237, "LOD3 Switch PC" },
            { 238, "LOD4 Switch PC" },
            { 239, "Aggressive Braking Allowed" }, // bool
            { 240, "Performance Parts Available" }, // bool
            { 241, "Paint Options Available" }, // bool
            { 242, "Model Options Available" }, // bool
            { 243, "Misc Options Available" }, // bool
            { 244, "Autobrake Activate" }, // bool
            { 245, "WS Decay Start Speed" },
            { 246, "WS Decay End Speed" },
            { 247, "WS Effect" },
            { 248, "Chase Cam Offset X" },
            { 249, "Chase Cam Offset Y" },
            { 250, "Chase Cam Offset Z" },
            { 251, "Garage Cam Heading" },
            { 252, "Garage Cam Elevation" },
            { 253, "Garage Cam Zoom" },
            { 254, "Level 1 Damage" },
            { 255, "Level 2 Damage" },
            { 256, "Level 3 Damage" },
            { 257, "L1 Damage Thrust" },
            { 258, "L2 Damage Thrust" },
            { 259, "L3 Damage Thrust" },
            { 260, "L1 Damage Pull" },
            { 261, "L2 Damage Pull" },
            { 262, "L3 Damage Pull" },
            { 263, "Spin Thrust Scale" },
            { 264, "Spin Thrust SR" },
            { 265, "Cornering Drag Reduce" },
            { 266, "Cornering Steer Assist" },
            { 267, "Vehicle Has Siren Lights" },
            { 268, "Vehicle Has Siren Sounds" },
            { 269, "Garage Cam Offset Y" },
            { 270, "Linked Thrust Multiply" },
            { 271, "Performance Parts Fitted" },
            { 272, "Model Options Fitted" },
            { 273, "Misc Options Fitted" },
            { 274, "Steer Sensitivity Steepness" },
            { 275, "Gear 0" },
            { 276, "Gear 1" },
            { 277, "Gear 2" },
            { 278, "Gear 3" },
            { 279, "Gear 4" },
            { 280, "Gear 5" },
            { 281, "Gear 6" },
            { 282, "Number Of Gears" },
            { 283, "Handling Performance Rating" },
            { 284, "Speed Performance Rating" },
            { 285, "Acceleration Performance Rating" },
            { 286, "Siren Speed" },
            { 287, "Siren Size 1" },
            { 288, "Siren Size 2" },
            { 289, "Siren Size 3" },
            { 290, "Siren 3 Brightness" },
            { 291, "Exhaust Size" },
            { 292, "Suspension Draw Minimum Clamp" }
        };
    }

    public class VehicleOverrideParameterSchemeTypes
    {
        public Dictionary<UInt16, string> DriverPL = new Dictionary<UInt16, string>
        {
            { 0, "float" },
            { 1, "float" },
            { 2, "float" },
            { 3, "float" },
            { 4, "float" },
            { 5, "float" },
            { 6, "float" },
            { 7, "float" },
            { 8, "float" },
            { 9, "float" },
            { 10, "float" },
            { 11, "float" },
            { 12, "float" },
            { 13, "float" },
            { 14, "float" },
            { 15, "float" },
            { 16, "float" },
            { 17, "float" },
            { 18, "float" },
            { 19, "float" },
            { 20, "float" },
            { 21, "float" },
            { 22, "float" },
            { 23, "float" },
            { 24, "float" },
            { 25, "float" },
            { 26, "float" },
            { 27, "float" },
            { 28, "float" },
            { 29, "bool" },
            { 30, "float" },
            { 31, "float" },
            { 32, "float" },
            { 33, "float" },
            { 34, "float" },
            { 35, "float" },
            { 36, "float" },
            { 37, "float" },
            { 38, "float" },
            { 39, "float" },
            { 40, "float" },
            { 41, "float" },
            { 42, "float" },
            { 43, "float" },
            { 44, "float" },
            { 45, "float" },
            { 46, "float" },
            { 47, "float" },
            { 48, "float" },
            { 49, "float" },
            { 50, "float" },
            { 51, "float" },
            { 52, "float" },
            { 53, "float" },
            { 54, "float" },
            { 55, "float" },
            { 56, "float" },
            { 57, "float" },
            { 58, "float" },
            { 59, "float" },
            { 60, "float" },
            { 61, "float" },
            { 62, "float" },
            { 63, "float" },
            { 64, "float" },
            { 65, "float" },
            { 66, "float" },
            { 67, "float" },
            { 68, "float" },
            { 69, "float" },
            { 70, "float" },
            { 71, "float" },
            { 72, "float" },
            { 73, "float" },
            { 74, "float" },
            { 75, "float" },
            { 76, "float" },
            { 77, "float" },
            { 78, "float" },
            { 79, "float" },
            { 80, "float" },
            { 81, "float" },
            { 82, "float" },
            { 83, "float" },
            { 84, "float" },
            { 85, "float" },
            { 86, "float" },
            { 87, "float" },
            { 88, "float" },
            { 89, "float" },
            { 90, "float" },
            { 91, "float" },
            { 92, "float" },
            { 93, "float" },
            { 94, "float" },
            { 95, "float" },
            { 96, "float" },
            { 97, "float" },
            { 98, "float" },
            { 99, "float" },
            { 100, "float" },
            { 101, "float" },
            { 102, "float" },
            { 103, "float" },
            { 104, "float" },
            { 105, "float" },
            { 106, "float" },
            { 107, "float" },
            { 108, "float" },
            { 109, "float" },
            { 110, "float" },
            { 111, "float" },
            { 112, "float" },
            { 113, "float" },
            { 114, "float" },
            { 115, "float" },
            { 116, "float" },
            { 117, "float" },
            { 118, "float" },
            { 119, "float" },
            { 120, "float" },
            { 121, "float" },
            { 122, "float" },
            { 123, "float" },
            { 124, "float" },
            { 125, "float" },
            { 126, "float" },
            { 127, "float" },
            { 128, "float" },
            { 129, "float" },
            { 130, "float" },
            { 131, "float" },
            { 132, "float" },
            { 133, "float" },
            { 134, "float" },
            { 135, "float" },
            { 136, "float" },
            { 137, "float" },
            { 138, "float" },
            { 139, "float" },
            { 140, "float" },
            { 141, "float" },
            { 142, "int" },
            { 143, "int" },
            { 144, "int" },
            { 145, "int" },
            { 146, "int" },
            { 147, "int" },
            { 148, "int" },
            { 149, "int" },
            { 150, "int" },
            { 151, "int" },
            { 152, "int" },
            { 153, "int" },
            { 154, "int" },
            { 155, "int" },
            { 156, "int" },
            { 157, "int" },
            { 158, "float" },
            { 159, "float" },
            { 160, "float" },
            { 161, "float" },
            { 162, "float" },
            { 163, "float" },
            { 164, "float" },
            { 165, "float" },
            { 166, "float" },
            { 167, "float" },
            { 168, "float" },
            { 169, "float" },
            { 170, "float" },
            { 171, "float" },
            { 172, "float" },
            { 173, "float" },
            { 174, "float" },
            { 175, "float" },
            { 176, "float" },
            { 177, "float" },
            { 178, "float" },
            { 179, "float" },
            { 180, "float" },
            { 181, "float" },
            { 182, "float" },
            { 183, "float" },
            { 184, "float" },
            { 185, "float" },
            { 186, "float" },
            { 187, "float" },
            { 188, "float" },
            { 189, "float" },
            { 190, "float" },
            { 191, "int" },
            { 192, "int" },
            { 193, "int" },
            { 194, "int" },
            { 195, "bool" },
            { 196, "bool" },
            { 197, "bool" },
            { 198, "bool" },
            { 199, "bool" },
            { 200, "bool" },
            { 201, "bool" },
            { 202, "bool" },
            { 203, "bool" },
            { 204, "bool" },
            { 205, "bool" },
            { 206, "bool" },
            { 207, "bool" },
            { 208, "float" },
            { 209, "bool" },
            { 210, "bool" },
            { 211, "bool" },
            { 212, "bool" },
            { 213, "bool" },
            { 214, "bool" },
            { 215, "bool" },
            { 216, "bool" },
            { 217, "bool" },
            { 218, "bool" },
            { 219, "bool" },
            { 220, "bool" },
            { 221, "float" },
            { 222, "float" },
            { 223, "float" },
            { 224, "float" },
            { 225, "float" },
            { 226, "float" },
            { 227, "float" },
            { 228, "float" },
            { 229, "float" },
            { 230, "float" },
            { 231, "float" },
            { 232, "float" },
            { 233, "float" },
            { 234, "float" },
            { 235, "float" },
            { 236, "float" },
            { 237, "float" },
            { 238, "float" },
            { 239, "bool" },
            { 240, "bool" },
            { 241, "bool" },
            { 242, "bool" },
            { 243, "bool" },
            { 244, "bool" },
            { 245, "float" },
            { 246, "float" },
            { 247, "float" },
            { 248, "float" },
            { 249, "float" },
            { 250, "float" },
            { 251, "float" },
            { 252, "float" },
            { 253, "float" },
            { 254, "float" },
            { 255, "float" },
            { 256, "float" },
            { 257, "float" },
            { 258, "float" },
            { 259, "float" },
            { 260, "float" },
            { 261, "float" },
            { 262, "float" },
            { 263, "float" },
            { 264, "float" },
            { 265, "float" },
            { 266, "float" },
            { 267, "float" },
            { 268, "float" },
            { 269, "float" },
            { 270, "float" },
            { 271, "float" },
            { 272, "float" },
            { 273, "float" },
            { 274, "float" },
            { 275, "float" },
            { 276, "float" },
            { 277, "float" },
            { 278, "float" },
            { 279, "float" },
            { 280, "float" },
            { 281, "float" },
            { 282, "float" },
            { 283, "float" },
            { 284, "float" },
            { 285, "float" },
            { 286, "float" },
            { 287, "float" },
            { 288, "float" },
            { 289, "float" },
            { 290, "float" },
            { 291, "float" },
            { 292, "float" }
        };
    }
}
