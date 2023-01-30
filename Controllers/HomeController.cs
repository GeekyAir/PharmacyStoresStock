using Microsoft.Ajax.Utilities;
using Pharmacy.Models;
using System.Linq;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Pharmacy.Controllers
{
    public class HomeController : Controller
    {
        pharmacyEntities Context = new pharmacyEntities();

        public ActionResult Index()
        {
            var IS = Context.Stocks.OrderBy(d => d.id_store).ToList();
            return View(IS);
        }
        public ActionResult Invoices()
        {
            var inv = Context.invoices.OrderBy(d => d.id_store).ToList();
            return View(inv);
        }
        public ActionResult AddInvoice()
        {
            var items = Context.items.OrderBy(d => d.id).ToList();
            var stores = Context.stores.OrderBy(d => d.id).ToList();
            Items_stores res = new Items_stores();
            res.Items = items;
            res.Stores = stores;
            return View(res);
        }
        public ActionResult SaveInvoice(invoice inv)
        {
            invoice tmp = new invoice();
            tmp.invoiceNO = inv.invoiceNO;
            tmp.date = inv.date;
            if (inv.quantity <= 0)
                inv.quantity = 1;
            if(Context.Stocks.Where(d=>d.id_store == inv.id_store && d.id_item == inv.id_item).FirstOrDefault() != null)
            {
                Context.Stocks.Where(d => d.id_store == inv.id_store && d.id_item == inv.id_item).FirstOrDefault().quantity += inv.quantity;
            }
            else
            {
                Stock st = new Stock();
                st.quantity = inv.quantity;
                st.id_store = inv.id_store;
                st.id_item = inv.id_item;
                Context.Stocks.Add(st);
            }
            tmp.id_store = inv.id_store;
            tmp.id_item = inv.id_item;
            tmp.quantity = inv.quantity;
            Context.invoices.Add(tmp);
            Context.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult DeleteInvoice(string invoiceNO)
        {
            invoice inv = Context.invoices.Where(d=> d.invoiceNO == invoiceNO).FirstOrDefault();
            Stock st = Context.Stocks.Where(d => d.id_store == inv.id_store && d.id_item == inv.id_item).FirstOrDefault();
            st.quantity -= inv.quantity;
            if (st.quantity == 0)
                Context.Stocks.Remove(st);
            Context.invoices.Remove(inv);
            Context.SaveChanges();
            return RedirectToAction("Invoices");
        }
        public ActionResult Items()
        {
            var items = Context.items.OrderBy(d => d.id).ToList();
            return View(items);
        }
        public ActionResult AddItem(string? New_Item)
        {
            if (New_Item != null)
            {
                item it = new item();
                it.item_name = New_Item;
                Context.items.Add(it);
                Context.SaveChanges();
                return RedirectToAction("Items");
            }
            else return View();
        }
        public ActionResult EditItem(string type, int? id, item? edited)
        {
            if (type == "edit")
            {
                item it = Context.items.Where(d => d.id == id).FirstOrDefault();
                return View(it);
            }
            else
            {
                item it = Context.items.Where(d => d.id == edited.id).FirstOrDefault();
                it.item_name = edited.item_name;
                Context.SaveChanges();
                return RedirectToAction("Items");
            }
        }
        public ActionResult DeleteItem(int id)
        {
            item it = Context.items.Where(d => d.id == id).FirstOrDefault();
            Context.items.Remove(it);
            Context.SaveChanges();
            return RedirectToAction("Items");
        }
        public ActionResult Stores()
        {
            var stores = Context.stores.OrderBy(d => d.id).ToList();
            return View(stores);
        }

        public ActionResult AddStore(string? New_Store)
        {
            if (New_Store != null)
            {
                store st = new store();
                st.store_name = New_Store;
                Context.stores.Add(st);
                Context.SaveChanges();
                return RedirectToAction("Stores");
            }
            else return View();
        }
        public ActionResult EditStore(string type, int? id, store? edited)
        {
            if (type == "edit")
            {
                store st = Context.stores.Where(d => d.id == id).FirstOrDefault();
                return View(st);
            }
            else
            {
                store st = Context.stores.Where(d => d.id == edited.id).FirstOrDefault();
                st.store_name = edited.store_name;
                Context.SaveChanges();
                return RedirectToAction("Stores");
            }
        }
        public ActionResult DeleteStore(int id)
        {
            store st = Context.stores.Where(d => d.id == id).FirstOrDefault();
            Context.stores.Remove(st);
            Context.SaveChanges();
            return RedirectToAction("Stores");
        }
    }
}