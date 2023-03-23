using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()

        {
            ML.Producto producto = new ML.Producto();
            ML.Result result = BL.Producto.GetAll(producto);


            producto.Productos = result.Objects;
            return View(producto);
        }

        //[HttpGet]
        //public ActionResult Form(int? idProducto) //2
        //{
        //    ML.Result resultArea = BL.Area.GetAll();
            

        //    ML.Producto producto = new ML.Producto();
           


        //    producto.Departamento = new ML.Departamento();
        //    producto.Departamento.Area = new ML.Area();
           


        //    if (resultArea.Correct)
        //    {
        //        producto.Departamento.Area = resultArea.Objects;
                
        //    }
        //    if (idProducto == null)
        //    {
        //        //add //formulario vacio
        //        return View(producto);
        //    }
        //    else
        //    {
        //        //getById
        //        ML.Result result = BL.Producto.GetById(idProducto.Value); //2

        //        if (result.Correct)
        //        {
        //            producto = (ML.Producto)result.Object;//unboxing


        //            producto.Departamento = new ML.Departamento();
        //            producto.Departamento.Area = new ML.Area();
        //            producto.Departamento.Area = resultArea.Objects;
        //            //update
        //            return View(producto);
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Ocurrio un error al consultar la información del usuario";
        //            return View("Modal");
        //        }


        //    }


        //}
        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            //HttpPostedFileBase file = Request.Files["fuImage"];
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ConvertToBytes(file);

                usuario.Imagen = Convert.ToBase64String(imagen);
            }


            ML.Result result = new ML.Result();
            if (ModelState.IsValid == true)
            {
                if (usuario.IdUsuario == 0)
                {
                    //add 
                    result = BL.Usuario.Add(usuario);

                    if (result.Correct)
                    {
                        ViewBag.Message = "Se completo el registro satisfactoriamente";
                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al insertar el registro";
                    }

                    return View("Modal");
                }
                else
                {

                    //update
                    //result = BL.Alumno.Add(alumno);

                    //if (result.Correct)
                    //{
                    //    ViewBag.Message = "Se actualizo la información satisfactoriamente";
                    //}
                    //else
                    //{
                    //    ViewBag.Message = "Ocurrio un error al actualizar el registro";
                    //}
                    return View("Modal");
                }
            }
            else
            {
                usuario.Rol = new ML.Rol();
                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

                ML.Result resultRol = BL.Rol.GetAll();
                ML.Result resultPais = BL.Pais.GetAll();

                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                return View(usuario);
            }

        }

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            //ML.Result result = BL.Alumno.Delete(idAlumno);
            return View();
        }

        [HttpPost]
        public JsonResult EstadoGetByIdPais(int idPais)
        {
            ML.Result result = new ML.Result();
            result = BL.Estado.EstadoGetByIdPais(idPais);

            return Json(result.Objects);
        }
        [HttpPost]
        public JsonResult MunicipioGetByIdEstado(int idEstado)
        {
            ML.Result result = new ML.Result();
            result = BL.Municipio.MunicipioGetByIdEstado(idEstado);

            return Json(result.Objects);
        }
        [HttpPost]
        public JsonResult ColoniaGetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = new ML.Result();
            result = BL.Colonia.ColoniaGetByIdMunicipio(idMunicipio);

            return Json(result.Objects);
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {

            using var fileStream = imagen.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }
    }
}
