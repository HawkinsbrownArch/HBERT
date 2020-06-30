using CarbonEmissionTool.Services;

namespace CarbonEmissionTool.Models
{
    public interface IDataCapture
    {
        void Upload(IProjectDetails projectDetails, CarbonDataCache carbonDataCache);
    }
}
