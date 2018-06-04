using System.Threading.Tasks;

namespace ChatApp.Lib.General
{
    public interface IInitializable
    {
        Task Initialize();
    }
}
