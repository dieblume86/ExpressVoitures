using ExpressVoitures.Models.Entities;
using ExpressVoitures.Models.ViewModels;

namespace ExpressVoitures.Models.Services.Interfaces
{
    public interface ICarService : IGenericEntityService<Car, CarViewModel>
    {
      //  List<Car> GetAllCars();
      // // List<CarViewModel> GetAllCarsViewModel();
      //  Car GetCarById(int id);
      // // CarViewModel GetCarByIdViewModel(int id);
      //  void AddCar();
      // // void SaveCar(CarViewModel product);
      //  void DeleteCar(int id);
      ////  List<string> CheckCarModelErrors(CarViewModel product);
      //  Task<Car> GetCar(int id);
      //  Task<IList<Car>> GetCar();
    }
}
