Create Procedure Update_tblUserMaster
(
@Id int,
@UserName nvarchar(20),
@Password nvarchar(100),
@Linkedwith nvarchar(100),
@Active bit,
@UpdatedBy int,
@UpdatedOn datetime
)
as
begin
Update tblUserMaster Set 
 UserName = @UserName ,
 Password = @Password ,
 Linkedwith = @Linkedwith ,
 Active = @Active ,
 UpdatedBy = @UpdatedBy ,
 UpdatedOn = @UpdatedOn
Where Id = @Id
end