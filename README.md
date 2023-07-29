# Teapot middleware
Always wanted someone to be 418'd? Now you can show 'em! A simple middleware to tell your clients that they are doing something ridiculous.

In this case 'ridiculous' being sending a request body with HTTP GET requests. Absurd, innit!?

## Usage

Just call `app.UseTeapot()` somewhere in the middleware configuration section. Like this:

```csharp
var app = builder.Build();

app.UseAuthentication();
app.UseTeapot();
app.UseAuthorization();

await app.RunAsync();
```

## Details and recommendations

I'd like to see you using this middleware in production, but alas, I advise you against it. 
I tried to make sure Teapot Middleware creates from little to none performance overhead - it doesn't read the whole request body, for example - 
in fact, as my research shows, even if it does, it's never more than 21B and the memory gets deallocated instantly thanks to PipeReader.

Even so, it's unlikely you want a just-for-fun middleware sitting there, especially if your API is not public. 

But, of course, you might consider using it in not performance critical scenarios...

---

With love, @eveloth
