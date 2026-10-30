# Passing Form Values to a Controller in ASP.NET Core MVC

**Student guide — based on the UserController example in MyMvcApp**

## Learning objectives

By the end of this activity, you should be able to:

- Explain the roles of a Model, View, and Controller.
- Create a form that accepts a user's first name, last name, and email.
- Receive submitted form values in a controller using model binding.
- Display the submitted values on another page.

## 1. Understand the flow

MVC stands for **Model–View–Controller**.

| Part | Responsibility in this example | File |
| --- | --- | --- |
| Model | Defines the values the form collects | `Models/UserModel.cs` |
| View | Displays the form and the submitted details | `Views/User/Create.cshtml` and `Views/User/Success.cshtml` |
| Controller | Handles requests and decides which view to display | `Controllers/UserController.cs` |

The browser and application communicate in this order:

```text
Browser requests /User/Create using GET
                  ↓
Controller returns the Create view
                  ↓
Student enters values and clicks Submit Form
                  ↓
Browser sends the values to /User/Create using POST
                  ↓
MVC binds the values to a UserModel object
                  ↓
Controller checks ModelState and redirects to Success
                  ↓
Browser requests Success and the submitted details appear
```

**GET** requests a page. **POST** sends form data to the application. In this example, both use `/User/Create`, but the HTTP method determines which action runs.

## 2. Start with an MVC project

If you are using the provided `MyMvcApp` project, continue to Step 3.

To create a new project with the same name, run these commands in a terminal with the .NET SDK installed:

```bash
dotnet new mvc -n MyMvcApp
cd MyMvcApp
```

The provided project targets .NET 9. If your project has a different name, replace `MyMvcApp` in the namespaces and `@model` declarations below.

MVC must be registered in `Program.cs`:

```csharp
builder.Services.AddControllersWithViews();
```

The existing application also defines this route:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
```

With this route, `/User/Create` selects `UserController` and its `Create` action. The `Controller` suffix is omitted from the URL.

## 3. Create the model

Create `Models/UserModel.cs`:

```csharp
namespace MyMvcApp.Models
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
```

Each property represents one value that the form will submit. For example, `FirstName` holds the first name entered by the user.

The code above follows the existing model. With nullable reference types enabled, you may see initialization warnings for these properties. You can initialize each property with `= string.Empty;`, for example:

```csharp
public string FirstName { get; set; } = string.Empty;
```

## 4. Create the controller

Create `Controllers/UserController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Models;

namespace MyMvcApp.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public IActionResult Create(int id, string name)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Success", model);
            }

            return View(model);
        }

        public IActionResult Success(UserModel model)
        {
            return View(model);
        }
    }
}
```

This reproduces the behavior of the existing controller, with unnecessary imports and comments omitted.

### The GET action displays the form

```csharp
[HttpGet]
public IActionResult Create(int id, string name)
{
    return View();
}
```

When you open `/User/Create`, MVC runs this method. `return View()` looks for `Views/User/Create.cshtml` by convention.

The `id` and `name` parameters are present in the existing example but are unused. They are not needed to display or submit this form. For a simpler classroom version, you can change the signature to:

```csharp
public IActionResult Create()
```

### The POST action receives the values

```csharp
[HttpPost]
public IActionResult Create(UserModel model)
```

MVC creates a `UserModel` object and matches submitted field names to its property names. This process is called **model binding**.

For example:

| Submitted field name | Example value | Controller receives |
| --- | --- | --- |
| `FirstName` | Ana | `model.FirstName` is `"Ana"` |
| `LastName` | Santos | `model.LastName` is `"Santos"` |
| `Email` | ana@example.com | `model.Email` is `"ana@example.com"` |

You do not need to manually read each form field or create the model object yourself.

### The controller checks the result

`ModelState.IsValid` checks whether binding and model validation succeeded. If valid, the controller redirects to `Success`. Otherwise, `return View(model)` redisplays the Create view with the submitted values.

The current model has no explicit validation attributes. Because this project enables nullable reference types, MVC normally treats its non-nullable string properties as implicitly required. Explicit validation rules and visible error messages are covered in the optional activity below.

## 5. Create the form view

Create a `User` folder inside `Views`, then create `Views/User/Create.cshtml`:

```cshtml
@model MyMvcApp.Models.UserModel

@{
    ViewData["Title"] = "Create User";
}

<h2>Create User</h2>

<form asp-controller="User" asp-action="Create" method="post">
    <div>
        <label asp-for="FirstName">First Name:</label>
        <input asp-for="FirstName" class="form-control" />
    </div>

    <div>
        <label asp-for="LastName">Last Name:</label>
        <input asp-for="LastName" class="form-control" />
    </div>

    <div>
        <label asp-for="Email">Email:</label>
        <input asp-for="Email" type="email" class="form-control" />
    </div>

    <button type="submit">Submit Form</button>
</form>
```

### What the important lines mean

| Code | Meaning |
| --- | --- |
| `@model MyMvcApp.Models.UserModel` | Declares the type of data this view works with. |
| `asp-controller="User"` | Targets `UserController`. |
| `asp-action="Create"` | Targets its `Create` action. |
| `method="post"` | Makes the browser send a POST request when the form is submitted. |
| `asp-for="FirstName"` | Connects the input to the model's `FirstName` property and generates the field name used for binding. |
| `type="email"` | Enables the browser's email input behavior and basic format checking. |
| `type="submit"` | Submits the form when the button is clicked. |

**The action attributes select the HTTP method.** The `asp-controller` and `asp-action` attributes generate the destination URL; `method="post"` makes the request use POST.

Conceptually, the FirstName input becomes HTML like this:

```html
<input type="text" id="FirstName" name="FirstName" value="" />
```

The `name="FirstName"` attribute is what allows MVC to match the submitted value to `UserModel.FirstName`.

Tag Helpers must be enabled in `Views/_ViewImports.cshtml`. The provided project already includes:

```cshtml
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

## 6. Create the success view

Create `Views/User/Success.cshtml`:

```cshtml
@model MyMvcApp.Models.UserModel

@{
    ViewData["Title"] = "Success";
}

<h2>Form Submitted Successfully!</h2>

<div>
    <h4>Submitted User Details:</h4>
    <hr />
    <p><strong>First Name:</strong> @Model.FirstName</p>
    <p><strong>Last Name:</strong> @Model.LastName</p>
    <p><strong>Email:</strong> @Model.Email</p>
</div>

<a asp-controller="User" asp-action="Create">Create Another User</a>
```

In the controller, `return View(model)` passes an object to the view. Inside the view, the uppercase `Model` property gives access to that object. For example, `@Model.FirstName` displays its first name.

### How the redirect passes the values

The existing POST action uses:

```csharp
return RedirectToAction("Success", model);
```

This produces a redirect, and the browser makes a new GET request. The model's properties are used as route values; with the current route, these properties appear as query parameters. A typical destination is:

```text
/User/Success?FirstName=Ana&LastName=Santos&Email=ana%40example.com
```

MVC then binds those query parameters to the `UserModel` parameter of `Success`.

The original C# object does not travel directly between requests. The values travel in the URL, and MVC builds another model object for the new request.

This pattern is called **Post/Redirect/Get**. Refreshing the resulting success page repeats the GET request instead of resubmitting the original POST. It does not prevent every possible duplicate submission.

This classroom example does **not save a user to a database**. The values are visible and editable in the URL. For an application that stores user records, save the data on the server and redirect using the saved record's identifier.

## 7. Run and test

From the project folder, run:

```bash
dotnet run
```

Use the address shown in the terminal and append `/User/Create`. For example, if the terminal shows `http://localhost:5000`, open:

```text
http://localhost:5000/User/Create
```

Use your application's actual port.

1. Enter `Ana` as the first name.
2. Enter `Santos` as the last name.
3. Enter `ana@example.com` as the email.
4. Click **Submit Form**.
5. Confirm that the success page displays all three values.
6. Click **Create Another User** to return to the form.

To see model binding happen, place a debugger breakpoint at `if (ModelState.IsValid)` in the POST action. Submit the form and inspect `model.FirstName`, `model.LastName`, and `model.Email`.

## 8. Optional: add clear validation messages

Once the basic flow works, replace the model with explicit validation rules:

```csharp
using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Please enter your first name.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your last name.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
    }
}
```

Inside the form, add a validation summary:

```cshtml
<div asp-validation-summary="All" class="text-danger"></div>
```

You can also place a message beside each input, using the matching property name:

```cshtml
<span asp-validation-for="FirstName" class="text-danger"></span>
```

The controller already returns `View(model)` when validation fails. These additions make the errors visible. Server-side validation runs when the form reaches the controller; browser validation may stop an invalid form before it is sent.

As a further improvement, add `[ValidateAntiForgeryToken]` to the POST action. The MVC Form Tag Helper generates the hidden token for this POST form; the attribute makes the controller validate it.

## 9. Common problems

| Problem | What to check |
| --- | --- |
| `/User/Create` is not found | Check the running application's address, the controller name, the action name, and the route in `Program.cs`. |
| The view cannot be found | Put `Create.cshtml` and `Success.cshtml` inside `Views/User/`. |
| Submitted values are empty | Check that input names match the model properties. Using `asp-for` generates those names for you. |
| The POST action does not run | Check `method="post"`. Also check whether browser validation blocked submission. |
| The form reappears instead of showing Success | Inspect `ModelState` for binding or validation errors; add the validation summary from Step 8. |
| `asp-for` appears unchanged in the generated HTML | Check the Tag Helper registration in `Views/_ViewImports.cshtml`. |
| The namespace cannot be found | Use your actual project's namespace consistently. |

## 10. Student exercise

Add a `Course` field to the example:

1. Add a `Course` property to `UserModel`.
2. Add a label and input with `asp-for="Course"` to the Create view.
3. Display `@Model.Course` in the Success view.
4. Submit the form and inspect the value in the POST action.

Answer these questions:

1. Which action displays the initial form?
2. Which action receives the submitted values?
3. How does MVC know which value belongs to `FirstName`?
4. What is the difference between `return View(model)` and `RedirectToAction("Success", model)`?
5. Does this example save anything to a database? Explain your answer.

### Teacher's answer key

1. The GET `Create` action.
2. The POST `Create(UserModel model)` action.
3. Model binding matches the input's submitted name to the model property name.
4. `View(model)` renders a view in the current request. `RedirectToAction` tells the browser to make a new request; in this example, the values are included in the destination URL.
5. No. The controller receives and displays values, but it contains no database save operation.
