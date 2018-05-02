# Html to Elmish [![Build status](https://ci.appveyor.com/api/projects/status/a9atdqi3pkcd755y/branch/master?svg=true)](https://ci.appveyor.com/project/MangelMaxime/html-to-elmish/branch/master)


## How to build ?

```shell
./fake.sh run build.fsx
```

The file needed to host the WebApp are in `./src/WebApp/output` folder.

## How to debug the WebApp ?

```shell
./fake.sh run build.fsx -t Watch
```

Browser to [http://localhost:8080](http://localhost:8080), the changes are applies directly to your application on save.

## How run the HtmlConverter tests ?

```shell
./fake.sh run build.fsx -t RunLiveTests
```

Browser to [http://localhost:8080](http://localhost:8080), you browser is reload each time you modify HtmlConverter or the tests.
