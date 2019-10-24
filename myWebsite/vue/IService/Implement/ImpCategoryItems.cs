using System.Collections.Generic;
using System.Linq;
using vue.DBModel;
using vue.ViewModel;
using codes = ViewModel.StateCodes.StateCode;

namespace vue.IService.Implement
{
    public class ImpCategoryItems : ICategoryItems
    {
        private HRCContext db = new HRCContext();

        /// <summary>
        /// 根据Name获取Category
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ReturnViewModel<IEnumerable<CategoryItems>> GetCategoryByName(string name) => new ReturnViewModel<IEnumerable<CategoryItems>>()
        {
            code = (int)codes.Success,
            data = db.CategoryItems.Where(x => x.CCategory == name),
        };
    }
}
