
Create Procedure Get_tblUserMaster
(
)
as
begin
Select Id, UserName, Password, Linkedwith, Active, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn 
 from tblUserMaster
end