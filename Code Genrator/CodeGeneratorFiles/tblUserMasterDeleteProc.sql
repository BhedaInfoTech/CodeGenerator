
Create Procedure Delete_tblUserMaster_By_Id
(
@Id int
)
as
begin
 Delete from tblUserMaster
 Where Id = @Id
end