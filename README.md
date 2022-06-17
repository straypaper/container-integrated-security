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
1. Run `kinit user@STRAYPAPER.COM` and provide the users's (principal's) password to authenticate to the domain and get an initial ticket-granting ticket.
2. Create a keytable file for the __application service account__ which can be created using the `ktutil` utility and the commands below:
   ```
   $ ktutil
   ktutil: add_entry -password -p svc-app@STRAYPAPER.COM -k 0 -e RC4-HMAC
   ktutil: write_kt client.keytab
   ktutil: exit
   ```
3. Run `klist` and verify the output is similar to:
   ```
   $ klist
   Ticket cache: KEYRING:persistent:1971801104:krb_ccache_kngqT7D
   Default principal: sduplooy@STRAYPAPER.COM
 
   Valid starting     Expires            Service principal
   06/17/22 16:47:12  06/18/22 02:47:12  krbtgt/STRAYPAPER.COM@STRAYPAPER.COM
	       renew until 06/24/22 16:47:09
   $
   ```
4. Create a secret in Docker Swarm using the keytable file using this command: 
   ```
   $ docker secret create client.keytab client.keytab
   ```

## Deploy and run the stacks
1. Compile the application using this command:
   ```
   dotnet build ./app/src/container-integrated-security
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

## References
1. https://github.com/ahmetgurbuz1/kerberos-sidecar
2. https://www.codeproject.com/Articles/1272546/Authenticate-NET-Core-Client-of-SQL-Server-with-In
