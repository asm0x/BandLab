namespace BandLab.Scaling.Interfaces;

public interface IScaleProcessing
{
    Task<string> Scaling(Scale data);
}
