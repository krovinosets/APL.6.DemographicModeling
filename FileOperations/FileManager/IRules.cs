using System.Collections;

namespace FileOperations.FileManager;

public interface IRules : IEnumerable
{
    double Get(int key);
}