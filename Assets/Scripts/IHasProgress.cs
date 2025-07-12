using System;

public interface IHasProgress
{
    event Action<float> OnProgressChanged;
}
