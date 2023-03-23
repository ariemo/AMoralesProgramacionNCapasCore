using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
      
            //Inyeccion de dependencias-- patron de diseño
            private readonly IConfiguration _configuration;
            private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

            public UsuarioController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
            {
                _configuration = configuration;
                _hostingEnvironment = hostingEnvironment;
            }
            [HttpGet]
        public ActionResult GetAll()

        {
            ML.Usuario usuario = new ML.Usuario();
            ML.Result result = BL.Usuario.GetAll(usuario);


            usuario.Usuarios = result.Objects;
            try
            {

                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Usuario/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                    }
                    usuario.Usuarios = result.Objects;
                }

            }
            catch (Exception ex)
            {
            }
            return View(usuario);
        }
        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAll(usuario);
            ML.Direccion direcciones = new ML.Direccion();
            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                direcciones.Direcciones = result.Objects;
                return View(usuario);

            }
            else
            {
                return View(usuario);
            }

        }

        [HttpGet]
        public ActionResult Form(int? idUsuario) //2
        {
            ML.Result resultPais = BL.Pais.GetAll();
            ML.Result resultRol = BL.Rol.GetAll();

            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();


            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();


            if ( resultPais.Correct && resultRol.Correct)
            {
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                usuario.Rol.Roles = resultRol.Objects;
            }
            if (idUsuario == null)
            {
                //add //formulario vacio
                return View(usuario);
            }
            else
            {
                //getById
                ML.Result result = BL.Usuario.GetById(idUsuario.Value); //2

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;//unboxing
                    usuario.Rol.Roles = resultRol.Objects;

                    usuario.Direccion = new ML.Direccion();
                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                    usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                    usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
                    //update
                    return View(usuario);
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al consultar la información del usuario";
                    return View("Modal");
                }


            }


        }
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Add", usuario);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se ha registrado el alumno";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Mensaje = "No se ha registrado el alumno";
                    return PartialView("Modal");
                }
            }


            //ML.Result result = new ML.Result();
            //if (ModelState.IsValid == true)
            //{
            //    if (usuario.IdUsuario == 0)
            //    {
            //        //add 
            //        result = BL.Usuario.Add(usuario);

            //        if (result.Correct)
            //        {
            //            ViewBag.Message = "Se completo el registro satisfactoriamente";
            //        }
            //        else
            //        {
            //            ViewBag.Message = "Ocurrio un error al insertar el registro";
            //        }

            //        return View("Modal");
            //    }
            //    else
            //    {


            //        result = BL.Usuario.Add(usuario);

            //        if (result.Correct)
            //        {
            //            ViewBag.Message = "Se actualizo la información satisfactoriamente";
            //        }
            //        else
            //        {
            //            ViewBag.Message = "Ocurrio un error al actualizar el registro";
            //        }
            //        return View("Modal");
            //    }
            //}
            //else
            //{
            //    usuario.Rol = new ML.Rol();
            //    usuario.Direccion = new ML.Direccion();
            //    usuario.Direccion.Colonia = new ML.Colonia();
            //    usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            //    usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            //    usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            //    ML.Result resultRol = BL.Rol.GetAll();
            //    ML.Result resultPais = BL.Pais.GetAll();

            //    usuario.Rol.Roles = resultRol.Objects;
            //    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;
            //    return View(usuario);
            //}

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
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string userName, string Password)
        {
            ML.Result result = BL.Usuario.GetByUserName(userName);
            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                if (Password == usuario.Password)
                {
                    return View();
                }
                else
                {
                    ViewBag.Message = "La contraseña no ee correcta";
                    return View("Modal");
                }
            }
            else
            {
                ViewBag.Message = "El Nombre de Usuario es incorrecto";
                return View("Modal");
            }
            
        }
    }
}
