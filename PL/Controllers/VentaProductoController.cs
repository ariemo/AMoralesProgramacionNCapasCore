using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class VentaProductoController : Controller
    {
        public ActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();
            producto.Departamento = new ML.Departamento();


            ML.Result result = BL.Producto.GetAll(producto);
            ML.Result resultDepartamento = BL.Departamento.GetAll();

            producto.Departamento.Departamentos = resultDepartamento.Objects;
            producto.Productos = result.Objects;

            return View(producto);
        }
        //[HttpPost]
        //public ActionResult GetAll(ML.Producto producto)
        //{
        //    producto.Nombre = (producto.Nombre == null) ? "" : producto.Nombre;

        //    //materia.Semestre.IdSemestre = (materia.Semestre.IdSemestre == null) ? "" : empleado.ApellidoPaterno;

        //    producto.Departamento.Area = producto.Departamento.Area == null ? "" : producto.Departamento.Area;

        //    ML.Result result = BL.Producto.GetAll(producto);
        //    producto.Productos = result.Objects;

         


        //    return View(producto);
        //}
        //public ActionResult Cart()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public ActionResult CartPost(ML.Producto producto)
        //{
        //    bool existe = false;
        //    ML.VentaProducto ventaProducto = new ML.VentaProducto();
        //    ventaProducto.Venta = new List<object>();

        //    if (HttpContext.Session.GetString("Producto") == null)
        //    {
        //        materia.Cantidad = materia.Cantidad = 1;
        //        materia.Subtotal = materia.Costo * materia.Cantidad;
        //        ventaMateria.VentaMaterias.Add(materia);
        //        HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaMateria.VentaMaterias));
        //        var session = HttpContext.Session.GetString("Producto");
        //    }
        //    else
        //    {
        //        GetCarrito(ventaMateria);
        //        foreach (ML.Materia venta in ventaMateria.VentaMaterias.ToList())
        //        {
        //            if (materia.IdMateria == venta.IdMateria)
        //            {
        //                venta.Cantidad = venta.Cantidad + 1;   
        //                venta.Subtotal = venta.Costo * venta.Cantidad;
        //                existe = true;
        //            }
        //            else
        //            {
        //                existe = false;
        //            }
        //            if (existe == true)
        //            {
        //                break;
        //            }
        //        }
        //        if (existe == false)
        //        {
        //            materia.Cantidad = materia.Cantidad = 1;
        //            materia.Subtotal = materia.Cantidad * materia.Costo;
        //            ventaMateria.VentaMaterias.Add(materia);
        //        }
        //        HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaMateria.VentaMaterias));
        //    }
        //    if (HttpContext.Session.GetString("Producto") != null)
        //    {
        //        ViewBag.Message = "Se ha agregado la materia a tu carrito!";
        //        return PartialView("Modal");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "No se pudo agregar tu producto ):";
        //        return PartialView("Modal");
        //    }

        //}
        //[HttpGet]
        //public ActionResult ResumenCompra(ML.VentaMateria ventaMateria)
        //{
        //    decimal costoTotal = 0;
        //    if (HttpContext.Session.GetString("Producto") == null)
        //    {
        //        return View();
        //    }
        //    else
        //    {
        //        ventaMateria.VentaMaterias = new List<object>();
        //        GetCarrito(ventaMateria);
        //        ventaMateria.Total = costoTotal;
        //    }

        //    return View(ventaMateria);
        //}
        //public ML.VentaMateria GetCarrito(ML.VentaMateria ventaMateria)
        //{
        //    var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));

        //    foreach (var obj in ventaSession)
        //    {
        //        ML.Materia objMateria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Materia>(obj.ToString());
        //        ventaMateria.VentaMaterias.Add(objMateria);
        //    }
        //    return ventaMateria;
        //}
        ////public JsonResult AddProducto(int idProducto)
        ////{

        ////}
    }
}
    

