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
    int n = 1;
    for (int i = 0; i < 10_000_000; i++)
    {
        while (denominator < n) {
            n *= 10;
            //Console.WriteLine(n);
        }
        if (denominator % n == 0)
        {
            digits[denominator] = 0;
            return;
        }
        denominator /= n;
        //Console.WriteLine(denominator);
    }
    digits[denominator] = n;
}

static int getDigitNON(int denominator) {
    int n = 1;
    int originalD = denominator;
    
    for (int i = 0; i < 10_000_000; i++)
    {
        while (denominator < n) {
            n *= 10;
            //Console.WriteLine(n);
        }
        denominator /= n;
        

        //Console.WriteLine(denominator);
    }
    if (denominator == originalD)
            return 0;
    return denominator;
}

Console.WriteLine(getDigitNON(13));


// static void MathKernel(Index1D index, ArrayView<int> floats, ArrayView<double> doubles, ArrayView<double> doubles2)
// {
//     doubles[index] = index*3;
//     doubles2[index] = index % 15;
//     floats[index] = index / 3;
// }
/*

using var context = Context.CreateDefault();

// For each available device...
var device = context.Devices[1];
// Create accelerator for the given device
using var accelerator = device.CreateAccelerator(context);
Console.WriteLine($"Performing operations on {accelerator}");

var kernel = accelerator.LoadAutoGroupedStreamKernel<
    Index1D, ArrayView<int>>(getDigit);

using var buffer = accelerator.Allocate1D<int>(100);

// Launch buffer.Length many threads
kernel((int)buffer.Length, buffer.View);

// Reads data from the GPU buffer into a new CPU array.
// Implicitly calls accelerator.DefaultStream.Synchronize() to ensure
// that the kernel and memory copy are completed first.
var data = buffer.GetAsArray1D();

foreach (var i in data) 
{
    Console.WriteLine(i);
}*/
