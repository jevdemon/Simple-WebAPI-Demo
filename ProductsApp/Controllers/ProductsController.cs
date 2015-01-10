// add a reference to our Models so was can use them in our Controller
using ProductsApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
// system.net.http includes HttpResponse
using System.Net.Http;
using System.Web.Http;

namespace ProductsApp.Controllers
{
    // Web API Controllers MUST inherit from : ApiController
    // Public methods in the Controller map to HTTP verbs and are called Actions.

    // HTTP verbs are mapped to Controllers in the WebApiConfig.cs in the App_Start directory.
    // When the Web API framework receives an HTTP request, it tries to match the URI against 
    // one of the route templates in the web.config. If no route matches, the client receives a 
    // 404 error.
    //
    // Here is a default route used by Web API (see WebApiConfig.cs in the App_Start directory):
   	//   routes.MapHttpRoute(
   	//         name: "API Default",
    //         routeTemplate: "api/{controller}/{id}",
    //         defaults: new { id = RouteParameter.Optional }
	//    );
    //
    // This means URLs must use this format: api/<controller-class-name>/<parameter>
    // We can remove the 'api' from the URL and replace it with anything we like such as:
    //   routeTemplate: "myServices/{controller}/{id}"  OR
    //   routeTemplate: "{controller}/{id}"

    // Web API looks at the HTTP verb, and then looks for an action whose name begins with that 
    // HTTP verb. e.g.: for HTTP GET, Web API looks for an action that starts with "Get..."
    // This only applies to GET, POST, PUT, and DELETE. 
    // Other HTTP methods can be enabled using attributes on the Controller:
    //          [HttpGet]
    //      	public Product FindProduct(id) {}

    //          [HttpPost]
    //      	public HttpResponseMessage AddProduct(id) {}

    //          [AcceptVerbs("GET", "HEAD")]
	//          public Product FindProduct(id) { }

    // WebDAV method:
	//          [AcceptVerbs("MKCOL")]
	//          public void MakeCollection() { }

    public class ProductsController : ApiController
    {
        // set up a simple array that can be manipulated by WebAPI
        static Product[] products = new Product[] 
        { 
            new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1M }, 
            new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M }, 
            new Product { Id = 4, Name = "Hitchiker's Guide to the Galaxy", Category = "Book", Price = 6.95M }, 
            new Product { Id = 5, Name = "HTC One", Category = "Phone", Price = 299M } 
        };
  
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        public HttpResponseMessage GetProduct(int id)
        {
            // search and try to match the id that was passed in
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Unable to find Product with an ID = " + id);
            }
            return Request.CreateResponse(HttpStatusCode.OK, product);
        }

        public HttpResponseMessage PostProduct(Product newprod)
        {
            // In this simple example we're using an array so we'll extend the array
            // and add the new entry to it.
            // Normally we would update a database/datastore here to create a new entry.
            // After creating the entry you send back the URI for the thing you created
            // in the HTTP Location header.

            // Get current array length and add 1 to it for the new length
            var newLen = products.Length + 1;
    
            // resize the array to the new length 
            // Note: this is not optimal - we're only doing this for this example
            Array.Resize(ref products, newLen);

            // add the new product to our newly added array element
            // (need to subtract 1 from our new array length because arrays are zero-based)
            products[newLen - 1] = newprod;

            // populate the new ID appropriately using value of the new array element as index
            products[newLen - 1].Id = newLen;
    
            // create the URI pointing to our new resource 
            var locHeader = Request.RequestUri + "/" + (products[newLen - 1].Id);

            // create response message, add HTTP status of "201" for CREATED
            var respMsg = Request.CreateResponse(HttpStatusCode.Created);

            // add URI pointing to new item in the HTTP Header's Location
            respMsg.Headers.Location = new Uri(locHeader);
            
            // return the response
            return respMsg;
  
        }
    }
}