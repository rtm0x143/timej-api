## timej/api
Репозиторий backend разработки для **Timej**

# Tech summary
Стандартный ASP.Net API проект с использованием Entity Framework для PostgreSQL.
### While we use gRPC 
При использовании RPC в качестве подхода к дизайну API используется [Grpc.AspNetCore](https://github.com/grpc/grpc-dotnet), [Buf](https://docs.buf.build/introduction). При помощи "/Protos/genprotos.py" происходит обоащение к **Buf** гдя генерации асетов.
```
cd ./Protos
python genprotos.py
```
