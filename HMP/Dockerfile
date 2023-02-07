# #FROM mcr.microsoft.com/windows/servercore:1803 as installer
# #
# #SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';$ProgressPreference='silentlyContinue';"]
# #
# #RUN Invoke-WebRequest -OutFile nodejs.zip -UseBasicParsing "https://nodejs.org/dist/v12.4.0/node-v12.4.0-win-x64.zip"; `Expand-Archive nodejs.zip -DestinationPath C:\; ` Rename-Item "C:\\node-v12.4.0-win-x64" c:\nodejs
# #
# #FROM mcr.microsoft.com/windows/nanoserver:1803
# #
# #WORKDIR C:\nodejs
# #COPY --from=installer C:\nodejs\ .
# #RUN SETX PATH C:\nodejs
# #RUN npm config set registry https://registry.npmjs.org/
# #
# #stage 1
# FROM stefanscherer/node-windows as node
# WORKDIR /src
# COPY AngularApp/ .
# RUN npm install
# ENV NODE_OPTIONS=--max_old_space_size=4096
# RUN npm run build
# #stage 2
# WORKDIR /src/dist/AngularApp
# RUN npm install http-server -g
# ENTRYPOINT ["http-server", "-p", "4200"]
