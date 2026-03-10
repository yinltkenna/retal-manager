Giai đoạn 1 – Chức năng bắt buộc (Core Tenant & Contract Management)

Mục tiêu: hệ thống quản lý được tenant và contract hoàn chỉnh.

1. Tenant Management

Chức năng bắt buộc

Tạo tenant
Cập nhật tenant
Xóa mềm tenant
Lấy danh sách tenant
Lấy chi tiết tenant
Upload avatar tenant

API đề xuất

POST /tenants
PUT /tenants/{id}
DELETE /tenants/{id}
GET /tenants
GET /tenants/{id}
PATCH /tenants/{id}/avatar

Luồng chính

Create Tenant

Admin / Staff
→ nhập thông tin tenant
→ validate dữ liệu
→ insert Tenant
→ return TenantId

2. Tenant Identity Document

Quản lý giấy tờ tùy thân.

Chức năng

Thêm giấy tờ
Cập nhật giấy tờ
Xóa giấy tờ
Xem danh sách giấy tờ
Upload ảnh giấy tờ

API

POST /tenants/{id}/identity-documents
PUT /identity-documents/{id}
DELETE /identity-documents/{id}
GET /tenants/{id}/identity-documents

Luồng thêm giấy tờ

Tenant / Admin
→ nhập thông tin CCCD / Passport
→ upload ảnh trước và sau
→ lưu TenantIdentityDocument

3. Tenant Temporary Residence

Quản lý đăng ký tạm trú.

Chức năng

Tạo đăng ký tạm trú
Cập nhật trạng thái
Gia hạn tạm trú
Xem danh sách tạm trú

API

POST /temporary-residences
PUT /temporary-residences/{id}
GET /tenants/{id}/temporary-residences

Trạng thái nên có

Pending
Registered
Expired
Cancelled

Luồng đăng ký tạm trú

Tenant ký hợp đồng
→ tạo TemporaryResidence
→ gửi thông tin đăng ký
→ cập nhật trạng thái

4. Contract Management

Quản lý hợp đồng thuê.

Chức năng bắt buộc

Tạo hợp đồng
Cập nhật hợp đồng
Xóa hợp đồng
Lấy danh sách hợp đồng
Lấy chi tiết hợp đồng
Cập nhật trạng thái hợp đồng

API

POST /contracts
PUT /contracts/{id}
DELETE /contracts/{id}
GET /contracts
GET /contracts/{id}
PATCH /contracts/{id}/status

Trạng thái hợp đồng nên có

Draft
Active
Expired
Terminated
Cancelled

Luồng tạo Contract

Admin
→ chọn Room
→ chọn Tenant đại diện
→ nhập giá thuê
→ nhập tiền cọc
→ nhập chu kỳ thanh toán
→ tạo Contract

5. Contract Member

Quản lý người ở trong hợp đồng.

Chức năng

Thêm tenant vào contract
Xóa tenant khỏi contract
Lấy danh sách tenant của contract

API

POST /contracts/{id}/members
DELETE /contracts/{id}/members/{tenantId}
GET /contracts/{id}/members

Luồng thêm member

Admin
→ chọn tenant
→ thêm vào ContractMember

6. Contract File

Quản lý file hợp đồng.

Chức năng

Upload file hợp đồng
Xóa file
Lấy danh sách file

API

POST /contracts/{id}/files
DELETE /contract-files/{id}
GET /contracts/{id}/files

Luồng upload file

FE → Media Service → trả FileId
→ Tenancy Service lưu ContractFile 

System function

Giai đoạn 2 – Quản lý vòng đời hợp đồng

Mục tiêu: xử lý gia hạn, kết thúc, lịch sử.

1. Contract Extension

Gia hạn hợp đồng.

Chức năng

Tạo yêu cầu gia hạn
Duyệt gia hạn
Xem lịch sử gia hạn

API

POST /contracts/{id}/extensions
GET /contracts/{id}/extensions

Luồng gia hạn

Contract gần hết hạn
→ tạo extension
→ cập nhật EndDate

2. Contract Termination

Chấm dứt hợp đồng.

Chức năng

Tạo yêu cầu kết thúc hợp đồng
Lưu lý do kết thúc
Tính tiền hoàn cọc

API

POST /contracts/{id}/termination
GET /contracts/{id}/termination

Luồng terminate contract

Admin / Tenant
→ nhập reason
→ xác định refundDeposit
→ update contract status

3. Contract Deposit Transaction

Quản lý tiền cọc.

Chức năng

Thu tiền cọc
Hoàn tiền cọc
Khấu trừ tiền cọc
Xem lịch sử giao dịch

API

POST /contracts/{id}/deposit-transactions
GET /contracts/{id}/deposit-transactions

TransactionType

Deposit
Refund
Deduct

Giai đoạn 3 – Điều khoản và lịch sử

Mục tiêu: quản lý rule và audit.

1. Contract Rule

Quản lý điều khoản hợp đồng.

Chức năng

Thêm điều khoản
Cập nhật điều khoản
Xóa điều khoản
Lấy danh sách điều khoản

API

POST /contracts/{id}/rules
PUT /contract-rules/{id}
DELETE /contract-rules/{id}
GET /contracts/{id}/rules

2. Contract History

Log thay đổi trạng thái.

Chức năng

Lưu lịch sử thay đổi
Xem lịch sử

API

GET /contracts/{id}/history

Luồng

Contract status change
↓
insert ContractHistory

Ví dụ

Draft → Active
Active → Extended
Active → Terminated

Giai đoạn triển khai đề xuất

Phase 1 – Core (Bắt buộc)

Tenant
TenantIdentityDocument
Contract
ContractMember
ContractFile

Mục tiêu

hệ thống quản lý tenant và contract cơ bản.

Phase 2 – Vòng đời hợp đồng

ContractExtension
ContractTermination
ContractDepositTransaction

Mục tiêu

quản lý lifecycle của contract.

Phase 3 – Rule và lịch sử

ContractRule
ContractHistory
TenantTemporaryResidence

Mục tiêu

hoàn thiện quản lý contract thực tế.

Các luồng quan trọng cần định nghĩa trước

Bắt buộc phải thiết kế kỹ

Tạo tenant
Thêm giấy tờ tenant
Tạo contract
Thêm tenant vào contract
Upload contract file
Gia hạn contract
Chấm dứt contract
Thu / hoàn tiền cọc
Thay đổi trạng thái contract