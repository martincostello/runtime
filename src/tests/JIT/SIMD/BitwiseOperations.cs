// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Collections.Generic;
using Point = System.Numerics.Vector2;
using Xunit;

namespace VectorMathTests
{
    public class Program
    {
        public const int DefaultSeed = 20010415;
        public static int Seed = Environment.GetEnvironmentVariable("CORECLR_SEED") switch
        {
            string seedStr when seedStr.Equals("random", StringComparison.OrdinalIgnoreCase) => new Random().Next(),
            string seedStr when int.TryParse(seedStr, out int envSeed) => envSeed,
            _ => DefaultSeed
        };

        static float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            double exponent = Math.Pow(2.0, random.Next(-32, 32));
            return (float)(mantissa * exponent);
        }

        static double[] GenerateDoubleArray(int size, Random random)
        {
            double[] arr = new double[size];
            for (int i = 0; i < size; ++i)
            {
                arr[i] = NextFloat(random);
            }
            return arr;
        }

        [Fact]
        public static int TestDouble()
        {
            Random random = new Random(Seed);
            double[] arr1 = GenerateDoubleArray(System.Numerics.Vector<double>.Count, random);
            double[] arr2 = GenerateDoubleArray(System.Numerics.Vector<double>.Count, random);
            var a = new System.Numerics.Vector<double>(arr1);
            var b = new System.Numerics.Vector<double>(arr2);
            var xorR = a ^ b;
            var andR = a & b;
            var orR = a | b;
            int Count = System.Numerics.Vector<double>.Count;
            for (int i = 0; i < Count; ++i)
            {
                Int64 f = BitConverter.DoubleToInt64Bits(a[i]);
                Int64 s = BitConverter.DoubleToInt64Bits(b[i]);
                Int64 r = f ^ s;
                double d = BitConverter.Int64BitsToDouble(r);
                if (xorR[i] != d)
                {
                    return 0;
                }
                r = f & s;
                d = BitConverter.Int64BitsToDouble(r);
                if (andR[i] != d)
                {
                    return 0;
                }
                r = f | s;
                d = BitConverter.Int64BitsToDouble(r);
                if (orR[i] != d && d == d)
                {
                    return 0;
                }
            }
            return 100;
        }

        static byte NextByte(Random random)
        {
            return (byte)random.Next(0, 255);
        }

        static byte[] GenerateByteArray(int size, Random random)
        {
            byte[] arr = new byte[size];
            for (int i = 0; i < size; ++i)
            {
                arr[i] = NextByte(random);
            }
            return arr;
        }

        [Fact]
        public static int TestBool()
        {
            Random random = new Random(Seed);
            byte[] arr1 = GenerateByteArray(System.Numerics.Vector<byte>.Count, random);
            byte[] arr2 = GenerateByteArray(System.Numerics.Vector<byte>.Count, random);
            var a = new System.Numerics.Vector<byte>(arr1);
            var b = new System.Numerics.Vector<byte>(arr2);

            var xorR = a ^ b;
            var andR = a & b;
            var orR = a | b;
            int Count = System.Numerics.Vector<byte>.Count;
            for (int i = 0; i < Count; ++i)
            {
                int d = a[i] ^ b[i];
                if (xorR[i] != d)
                {
                    return 0;
                }
                d = a[i] & b[i];
                if (andR[i] != d)
                {
                    return 0;
                }
                d = a[i] | b[i];
                if (orR[i] != d)
                {
                    return 0;
                }
            }
            return 100;
        }
    }
}
