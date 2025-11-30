namespace BandLab.Scaling.Interfaces;

public interface IScaleTasks
{
    ValueTask RunScale(string file, string path);
}
