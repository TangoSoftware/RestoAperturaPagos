# Resto Apertura Pagos
Este proyecto es una solución de apertura, para poder realizar Pre Pagos (como Giftcard, Pipol y otros) y que se integre con el sistema Restô.

## Tipo de solución
La solución esta desarrollada en .Net Core, y funciona como aplicación de consola o servicio de Windows.

## Características
- Es un servicio Rest, que antiende a un puerto a especificar en el código fuente.
- Código completamente documentado.
- Utiliza las librerias de Autofac y log4net.
- Fácil de incorporar otras empresas de prepago.

## ¿cómo ampliar funcionalidad?
- Incorporar un nuevo controlador: Respetar el método *Pagar* con sus parámetros. Dado que es el punto de entrada y de entendimiento de Tango Resto con las cuentas de Tesorería.
 
- Appsettings.js: Seguramente van a necesitar credenciales de acceso para conectarse a un nuevo proveedor de pagos. Para eso los parámetros deberán incluirlo en este archivo.

- Crear la clase [MedioPagoNuevo]Config.cs: En funcion del punto anterior, tiene que crear una clase de configuracion donde se mapee sus  propiedades con lo que se definió en el archivo AppSettings.cs

- Modificar Startup.cs: Hay que incluir la inyeccion de dependencia de la clase [mediopagonuevo]Config.cs 

```
            #region Inyección nativa Aspnet core. Se agrega las secciones de appSettings.
            services.Configure<AmipassConfig>(Configuration.GetSection("Amipass"));
            services.Configure<PipolConfig>(Configuration.GetSection("Pipol"));
            services.Configure<[MedioPagoNuevo]Config>(Configuration.GetSection("MedioPagoNuevo"));
            #endregion
```

- Crear Class Library como plugin al servicio: Para que el código quede limpio, cada empresa de prepago se crea un proyecto nuevo, de tipo class library, en la carpeta services. En este proyecto se tiene que implementar la Interface y la Clase del mismo. Donde se deberá implementar la conexión a la empresa de prepago y consumir un monto.

- Mapping de respuesta: En la carpeta Mapping del Servicio hay una clase RespuestaMapping, en la misma se deberá implementar el mapeo de la respuesta del plugin nuevo implementado a la clase estandard de salida RespuestaDto

- Modificar la clase ServiceRegisterHelper.cs: Esta clase tiene el conocimiento de clases e interfaces con el fin de realizar la inyección de dependencia del proyecto. Para eso deberá modificar la siguiente confiiguración

```
            builder.RegisterType<Amipass_Imp>().As<IAmipass>();
            builder.RegisterType<Pipol_Imp>().As<IPipol>();
            builder.RegisterType<[MedioPagoNuevo]_Imp>().As<I[MedioPagoNuevo]>();
```

## ¿Cómo depurar el servicio?
- Correr el servicio como consola.
- Utilizando la herrmienta Advance REST Client, configurar
  - Method POST
  - URL: http://localhost:5000/api/PagoPipol/Pagar
  - BODY:  
    *Body ContentType:* application/Json
    *Editor view:* Raw input
    *Format Json*
    ```
{
  "comanda": "55",
  "monto": "100",
  "codigo": "eQR01PIPOL31"
}
    ```
   
    
  
  
  
