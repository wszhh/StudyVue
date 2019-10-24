using System.Collections.Generic;
using vue.DBModel;
using vue.ViewModel;

namespace vue.IService
{
    public interface ICategoryItems
    {

        /// <summary>
        /// 根据Name获取Category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ReturnViewModel<IEnumerable<CategoryItems>> GetCategoryByName(string name);
    }
}
