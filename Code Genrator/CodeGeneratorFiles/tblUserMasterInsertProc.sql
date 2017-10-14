Create Procedure Insert_tblUserMaster
(
@UserName nvarchar(20),
@Password nvarchar(100),
@Linkedwith nvarchar(100),
@Active bit,
@CreatedBy int,
@CreatedOn datetime,
@UpdatedBy int,
@UpdatedOn datetime
)
as
begin
Insert into tblUserMaster
(
UserName,
Password,
Linkedwith,
Active,
CreatedBy,
CreatedOn,
UpdatedBy,
UpdatedOn
)
values
(
@UserName,
@Password,
@Linkedwith,
@Active,
@CreatedBy,
@CreatedOn,
@UpdatedBy,
@UpdatedOn
)
end