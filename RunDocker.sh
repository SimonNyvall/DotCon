#!/bin/bash

dotnet test

docker build -t dotcon-image .

docker run -it dotcon-image