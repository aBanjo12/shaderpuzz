using System.Numerics;
using System.Reflection.Metadata;
using ILGPU;
using ILGPU.IR.Analyses;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

// int[] digits = new int[denominator];
//     int k = 0, n = 1;
//     do {
//         n *= 10;
//         digits[k++] = n / denominator;
//         n = n % denominator;
//     }while(n != 1);
//     int[] period = new int[k];
//     for(n = 0; n < k; ++n) {
//         period[n] = digits[n];
//     }
//     return period.Length;

static void getDigit(Index1D denominator, ArrayView<int> digits) {
    if (denominator == 0)
        return;
    int n = 1;
    
    for (int i = 0; i < 10_000_000; i++)
    {
        n %= denominator;
        n *= 10;

        //Console.WriteLine(denominator);
    }
    // if (denominator == originalD)
    //         return 0;
    digits[denominator] = n/denominator;
}

static int getDigitNON(int denominator) {
    int n = 1;
    int originalD = denominator;
    
    for (int i = -3; i < 10_000_000; i++)
    {
        n %= denominator;
        n *= 10;

        //Console.WriteLine(denominator);
    }
    // if (denominator == originalD)
    //         return 0;
    return n/denominator;
}

// while (true)
// {
//     Console.WriteLine(getDigitNON(int.Parse(Console.ReadLine())));
        
// }

// int count = 0;
// for (int i = 1; i < 100; i++)
// {
//     count += getDigitNON(i);
// }

// Console.WriteLine(count);


// static void MathKernel(Index1D index, ArrayView<int> floats, ArrayView<double> doubles, ArrayView<double> doubles2)
// {
//     doubles[index] = index*3;
//     doubles2[index] = index % 15;
//     floats[index] = index / 3;
// }


using var context = Context.CreateDefault();

Console.WriteLine("num to count to:");
int num = int.Parse(Console.ReadLine());

Console.WriteLine("gpu index:");
int index = int.Parse(Console.ReadLine());

// For each available device...
var device = context.Devices[index];
// Create accelerator for the given device
using var accelerator = device.CreateAccelerator(context);
Console.WriteLine($"Performing operations on {accelerator}");

var kernel = accelerator.LoadAutoGroupedStreamKernel<
    Index1D, ArrayView<int>>(getDigit);

using var buffer = accelerator.Allocate1D<int>(num);

// Launch buffer.Length many threads
kernel((int)buffer.Length, buffer.View);

// Reads data from the GPU buffer into a new CPU array.
// Implicitly calls accelerator.DefaultStream.Synchronize() to ensure
// that the kernel and memory copy are completed first.
int[] data = buffer.GetAsArray1D();

BigInteger total = 0;

foreach (int i in data) 
{
    if (i < 1 || i > 9)
        continue;
    //Console.WriteLine(i);
    total += i;
}
Console.WriteLine(total);
