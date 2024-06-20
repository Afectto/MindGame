using System;
using System.Linq;

[Serializable]
public class SerializableArray2D<T>
{
    public readonly T[] ArrayFlat;
    public readonly int ElementSize;

    public SerializableArray2D(int elementSize, T[] arrayFlat)
    {
        ElementSize = elementSize;
        ArrayFlat = arrayFlat;
    }

    public SerializableArray2D(T[,] array2D) : this(array2D.GetLength(1), array2D.Cast<T>().ToArray()) { }

    public SerializableArray2D(int elementSize, int elementCount) : this(elementSize, new T[elementCount]){}

    public T this[int x, int y]
    {
        get => ArrayFlat[x * ElementSize + y];
        set => ArrayFlat[x * ElementSize + y] = value;
    }

    public T[] GetArrayFlat()
    {
        return ArrayFlat;
    }

    public int GetElementSize()
    {
        return ElementSize;
    }
}