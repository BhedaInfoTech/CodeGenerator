
Create Procedure Get_tblUserMaster_By_Id
(
@Id int
)
as
begin
Select Id, UserName, Password, Linkedwith, Active, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn 
 from tblUserMaster
 where  Id = @Id
end