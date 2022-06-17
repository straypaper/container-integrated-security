# Containers and Integrated Windows Authentication
Example code to connect to Microsoft SQL Server from a Linux container using Integrated Windows Authentication.

## Article
The problem and solution is discussed on the [Straypaper Blog](https://straypaper.com/containers-and-integrated-security/).

## Notes
The example reference the `STRAYPAPER.COM` domain and should be changed to match your own domain.

## Prerequisites
1. One or more Linux container hosts running Docker in Docker Swarm mode;
2. A Windows Active Directory configured with the following:
* An application service account, for example: `STRAYPAPER\svc-app`;
* A SQL Server service account, for example: `STRAYPAPER\svc-sql`;
* A Service Principal Name (SPN) registered in Active Directory for the SQL Server which can be created using this example command: 
  ```
  $ setspn -A MSSQLSvc/sp-sql-001.straypaper.com:1433 STRAYPAPER\svc-sql
  ```
3. A keytable file (.keytab) with the credentials for the `STRAYPAPER\svc-app` service account.

## Configuration
1. Create a keytable file for the application service account which can be created using the `kutil` utility and the commands below:
   ```
   $ ktutil
   ktutil: add_entry -password -p svc-app@STRAYPAPER.COM -k 1 -e RC4-HMAC
   ktutil: write_kt client.keytab
   ktutil: exit
   ```
2. Create a secret in Docker Swarm using the keytable file using this command: 
   ```
   $ docker secret create client.keytab client.keytab
   ```

## Deploy and run the stacks
1. Compile the application using this command:
   ```
   dotnet build ./app
   ```
2. Build the __kerberos-sidecar__ and __app__ containers using this command:
   ```
   $ docker-compose build
   ```
3. Deploy the kerberos-sidecar container to Docker Swarm using this command:
   ```
   $ docker stack deploy -c kerberos-sidecar/kerberos-sidecar-stack.yml kerberos-sidecar
   ```
4. Deploy the app container to Docker Swarm using this command:
   ```
   $ docker stack deploy -c app/app-stack.yml app
   ```
