using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Web;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using WebAPICode.Helpers;

namespace BAL.Repositories
{

    public class menuRepository : BaseRepository
    {

        public menuRepository()
            : base()
        {
            DBContext = new db_a74425_premiumposEntities();

        }

        public menuRepository(db_a74425_premiumposEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }
        public RspMenu GetMenu(int locationID,int UserID)
        {
            var CategoryLst = new List<CategoryBLL>();
            var ItemLst = new List<ItemBLL>();
            var SubCategoryLst = new List<SubCategoryBLL>();
            var lstModifier = new List<ModifierBLL>();
            var lstVariant = new List<VariantsBLL>();
            var lstIM= new List<string>();
            var rsp = new RspMenu();
            try
            {
                var catlist = DBContext.sp_GetCategory_menu(locationID).ToList();
                var subcatlist = DBContext.sp_GetSubCategory_menu(locationID).ToList();
                var itemslist = DBContext.sp_GetItem_menu(locationID).ToList();
                var modifierlist = DBContext.sp_GetModifiersForItem_menu(UserID).ToList();
                var variantlist = DBContext.sp_GetVariantsForItem_menu(UserID).ToList();

                if (catlist != null && catlist.Count() > 0)
                {
                    #region category
                    CategoryLst = new List<CategoryBLL>();
                    foreach (var Cat in catlist)
                    {
                        #region subcategory
                        SubCategoryLst = new List<SubCategoryBLL>();
                        foreach (var SubCat in subcatlist.Where(x => x.StatusID == 1 && x.CategoryID == Cat.ID).OrderBy(x => x.DisplayOrder).ToList())
                        {
                            #region item
                            ItemLst = new List<ItemBLL>();
                            foreach (var item in itemslist.Where(x => x.StatusID == 1 && x.SubCategoryID == SubCat.ID).OrderBy(x => Guid.NewGuid()).ToList())
                            {

                                lstModifier = new List<ModifierBLL>();
                                foreach (var modifiers in modifierlist.Where(x => x.StatusID == 1 && x.ItemID == item.ID).OrderBy(x => x.DisplayOrder).ToList())
                                {
                                    lstModifier.Add(new ModifierBLL
                                    {
                                        ID = modifiers.ID,
                                        Name = modifiers.Name,
                                        ArabicName = modifiers.ArabicName,
                                        Description = modifiers.Description,
                                        Type = modifiers.Type,
                                        Barcode = modifiers.Barcode,
                                        SKU = modifiers.SKU,
                                        Price = Convert.ToDouble(modifiers.Price),
                                        StatusID = Convert.ToInt32(modifiers.StatusID)

                                    });
                                }
                                lstVariant = new List<VariantsBLL>();
                                foreach (var variant in variantlist.Where(x => x.StatusID == 1 && x.ItemID == item.ID).OrderBy(x => x.DisplayOrder).ToList())
                                {
                                    lstVariant.Add(new VariantsBLL
                                    {
                                        ID = variant.ID,
                                        Name = variant.Name,
                                        ArabicName = variant.ArabicName,
                                        Description = variant.Description,
                                        Type = variant.Type,
                                        Barcode = variant.Barcode,
                                        SKU = variant.SKU,
                                        Price = Convert.ToDouble(variant.Price),
                                        StatusID = Convert.ToInt32(variant.StatusID)
                                    });
                                }

                                lstIM = new List<string>();
                                lstIM.Add(item.Image == null ? "" : ConfigurationSettings.AppSettings["AdminURL"].ToString() + item.Image.Replace(" ", "%20"));

                                var random = new Random();
                                var randomIF = new List<string> { "true","false"};
                             
                                ItemLst.Add(new ItemBLL
                                {
                                    ID = item.ID,
                                    Name = item.Name,
                                    Description = item.Description,
                                    Image = item.Image == null ? "" : ConfigurationSettings.AppSettings["AdminURL"].ToString() + item.Image.Replace(" ", "%20"),
                                    ItemType = item.ItemType,
                                    SubCategoryID = item.SubCategoryID,
                                    NameOnReceipt = item.NameOnReceipt,
                                    StatusID = item.StatusID,
                                    Barcode = item.Barcode,
                                    SKU = item.SKU,
                                    Price = item.Price,
                                    Cost = item.Cost,
                                    Modifiers = lstModifier,
                                    Variants = lstVariant,
                                    ItemImages=lstIM.ToArray(),
                                    DisplayOrder=item.DisplayOrder,
                                    IsFeatured= false
                                });
                            }
                            SubCategoryLst.Add(new SubCategoryBLL
                            {
                                SubCategoryID = SubCat.ID,
                                CategoryID = SubCat.CategoryID,
                                CategoryName = Cat.Name,
                                Name = SubCat.Name,
                                Description = SubCat.Description,
                                Image = SubCat.Image == null ? "" : ConfigurationSettings.AppSettings["AdminURL"].ToString() + SubCat.Image.Replace(" ", "%20"),
                                StatusID = SubCat.StatusID,
                                Items = ItemLst
                            });
                            #endregion items
                        }
                        CategoryLst.Add(new CategoryBLL
                        {
                            ID = Cat.ID,
                            Name = Cat.Name,
                            Description = Cat.Description,
                            Image = Cat.Image == null ? "" : ConfigurationSettings.AppSettings["AdminURL"].ToString() + Cat.Image.Replace(" ", "%20"),

                            StatusID = Cat.StatusID,
                            LocationID = Cat.LocationID,
                            Subcategories = SubCategoryLst
                        });
                        #endregion subcategory
                    }
                    #endregion category
                }
                rsp.Categories = CategoryLst;
                rsp.status = 1;
                rsp.description = "Success";


                return rsp;
            }
            catch (Exception ex)
            {
                rsp.Categories = new List<CategoryBLL>();
                rsp.status = 0;
                rsp.description = "Failed";
                return rsp;
            }
        }
        //public RspMenu GetMenuV2(int brandID)
        //{
        //    var bll = new List<CategoryBLL>();
        //    var lstItem = new List<ItemBLL>();
        //    var lstModifier = new List<ModifierBLL>();
        //    var rsp = new RspMenu();
        //    try
        //    {
        //        var ds = GetMenu_ADO(brandID);
        //        var _dsCategory = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0])).ToObject<List<CategoryBLL>>();
        //        var _dsItem = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[1])).ToObject<List<ItemBLL>>();
        //        var _dsModifier = JArray.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[2])).ToObject<List<ModifierBLL>>();

        //        foreach (var i in _dsCategory)
        //        {
        //            lstItem = new List<ItemBLL>();
        //            foreach (var j in _dsItem.Where(x => x.StatusID == 1 && x.CategoryID == i.CategoryID))
        //            {
        //                lstModifier = new List<ModifierBLL>();
        //                foreach (var k in _dsModifier.Where(x => x.StatusID == 1 && x.ItemID == j.ItemID))
        //                {
        //                    lstModifier.Add(new ModifierBLL
        //                    {
        //                        Name = k.Name,
        //                        StatusID = k.StatusID,
        //                        ArabicName = k.ArabicName,
        //                        Description = k.Description,
        //                        Image = k.Image == null ? "" : ConfigurationManager.AppSettings["AdminURL"].ToString() + k.Image,
        //                        LastUpdatedBy = k.LastUpdatedBy,
        //                        LastUpdatedDate = k.LastUpdatedDate,
        //                        Price = k.Price,
        //                        BrandID = k.BrandID,
        //                        ModifierID = k.ModifierID
        //                    });
        //                }

        //                lstItem.Add(new ItemBLL
        //                {
        //                    Name = j.Name,
        //                    StatusID = j.StatusID,
        //                    ArabicName = j.ArabicName,
        //                    Barcode = j.Barcode,
        //                    CategoryID = j.CategoryID,
        //                    Cost = j.Cost,
        //                    Description = j.Description,
        //                    DisplayOrder = j.DisplayOrder,
        //                    Image = j.Image == null ? "" : ConfigurationManager.AppSettings["AdminURL"].ToString() + j.Image,
        //                    ItemID = j.ItemID,
        //                    ItemType = j.ItemType,
        //                    LastUpdatedBy = j.LastUpdatedBy,
        //                    LastUpdatedDate = j.LastUpdatedDate,
        //                    Price = j.Price,
        //                    SKU = j.SKU,
        //                    UnitID = j.UnitID,
        //                    Calories = j.Calories,
        //                    IsSoldOut = false,
        //                    modifiers = lstModifier
        //                });
        //            }
        //            bll.Add(new CategoryBLL
        //            {
        //                BrandID = i.BrandID,
        //                ArabicName = i.ArabicName,
        //                LastUpdatedDate = i.LastUpdatedDate,
        //                LastUpdatedBy = i.LastUpdatedBy,
        //                CategoryID = i.CategoryID,
        //                Description = i.Description,
        //                DisplayOrder = i.DisplayOrder,
        //                LocationID = i.BrandID,
        //                Name = i.Name,
        //                Image = i.Image,
        //                StatusID = i.StatusID,
        //                items = lstItem
        //            });
        //        }

        //        rsp.categories = bll;
        //        rsp.status = 1;
        //        rsp.description = "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        rsp.categories = bll;
        //        rsp.status = 0;
        //        rsp.description = "Failed";
        //    }
        //    return rsp;
        //}
        public DataSet GetMenu_ADO(int BrandID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] p = new SqlParameter[1];
                p[0] = new SqlParameter("@BrandID", BrandID);
                ds = (new DBHelper().GetDatasetFromSP)("sp_GetMenu_api", p);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
