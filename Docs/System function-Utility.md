Giai đoạn 1 – Chức năng bắt buộc (Core Utility Management)

Mục tiêu: hệ thống quản lý được loại tiện ích, cấu hình giá và đồng hồ điện nước.

1. Utility Type Management

Quản lý loại tiện ích.

Ví dụ

Điện
Nước
Internet
Rác

Chức năng bắt buộc

Tạo UtilityType
Cập nhật UtilityType
Xóa UtilityType
Lấy danh sách UtilityType
Xem chi tiết UtilityType

API đề xuất

POST /utility-types
PUT /utility-types/{id}
DELETE /utility-types/{id}
GET /utility-types
GET /utility-types/{id}

Luồng chính

Create UtilityType

Admin
→ nhập tên tiện ích
→ validate dữ liệu
→ insert UtilityType
→ return UtilityTypeId

2. Utility Config (Giá tiện ích)

Quản lý giá tiện ích theo chi nhánh.

Chức năng bắt buộc

Tạo cấu hình giá
Cập nhật cấu hình giá
Lấy giá hiện tại
Lấy lịch sử giá

API

POST /utility-configs
PUT /utility-configs/{id}
GET /utility-configs/current?branchId=&utilityTypeId=
GET /utility-configs/history?branchId=&utilityTypeId=

Luồng xác định giá tiện ích

GetCurrentUtilityPrice

→ tìm UtilityConfig

WHERE StartDate <= now
AND (EndDate IS NULL OR EndDate >= now)

ORDER BY StartDate DESC

3. Utility Fixed Charge

Quản lý phí cố định hàng tháng.

Ví dụ

Internet
Rác
Phí vệ sinh

Chức năng

Tạo phí cố định
Cập nhật phí cố định
Lấy phí hiện tại
Lấy lịch sử phí

API

POST /utility-fixed-charges
PUT /utility-fixed-charges/{id}
GET /utility-fixed-charges/current?branchId=&utilityTypeId=
GET /utility-fixed-charges/history?branchId=&utilityTypeId=

Luồng

GetCurrentFixedCharge

→ tìm UtilityFixedCharge
WHERE StartDate <= now
AND (EndDate IS NULL OR EndDate >= now)

Giai đoạn 2 – Quản lý đồng hồ tiện ích

Mục tiêu: quản lý meter điện nước cho phòng.

1. Utility Meter Management

Quản lý đồng hồ.

Ví dụ

Điện phòng 101
Nước phòng 101

Chức năng

Tạo meter
Cập nhật meter
Xóa meter
Lấy danh sách meter
Lấy meter theo phòng

API

POST /utility-meters
PUT /utility-meters/{id}
DELETE /utility-meters/{id}
GET /utility-meters
GET /rooms/{roomId}/meters

Trạng thái meter nên có

Active
Inactive
Removed

Luồng tạo meter

Admin
→ chọn Room
→ chọn UtilityType
→ nhập MeterCode
→ tạo UtilityMeter

Giai đoạn 3 – Ghi chỉ số điện nước

Mục tiêu: quản lý meter reading và usage.

1. Meter Reading

Ghi chỉ số điện nước.

Chức năng

Ghi chỉ số
Cập nhật chỉ số
Lấy lịch sử chỉ số
Upload ảnh công tơ

API

POST /meter-readings
PUT /meter-readings/{id}
GET /meters/{id}/readings

Luồng ghi chỉ số

Staff

→ nhập CurrentValue
→ hệ thống lấy PreviousValue
→ tính Usage

Usage = CurrentValue - PreviousValue

→ insert MeterReading

2. Utility Reading History

Theo dõi thay đổi chỉ số.

Chức năng

Ghi log khi chỉnh sửa meter reading
Xem lịch sử chỉnh sửa

API

GET /meter-readings/{id}/history

Luồng

MeterReading update
↓
insert UtilityReadingHistory

Giai đoạn 4 – Điều chỉnh chỉ số

Mục tiêu: xử lý sai số khi ghi.

1. Utility Adjustment

Chức năng

Tạo adjustment
Xem lịch sử adjustment

API

POST /meter-readings/{id}/adjustments
GET /meter-readings/{id}/adjustments

Ví dụ

Nhập sai chỉ số
Sai do thiết bị

Luồng

Admin / Staff

→ nhập AdjustmentAmount
→ nhập Reason
→ insert UtilityAdjustment

Giai đoạn triển khai đề xuất

Phase 1 – Core Utility

UtilityType
UtilityConfig
UtilityFixedCharge

Mục tiêu

quản lý loại tiện ích và giá.

Phase 2 – Meter Management

UtilityMeter

Mục tiêu

quản lý đồng hồ điện nước.

Phase 3 – Meter Reading

MeterReading
UtilityReadingHistory

Mục tiêu

ghi và theo dõi chỉ số.

Phase 4 – Adjustment

UtilityAdjustment

Mục tiêu

xử lý sai lệch chỉ số.

Các luồng quan trọng cần định nghĩa trước

Bắt buộc phải thiết kế kỹ

Tạo UtilityType
Cấu hình giá tiện ích
Xác định giá tiện ích hiện tại
Tạo UtilityMeter
Ghi chỉ số điện nước
Tính usage
Upload ảnh công tơ
Chỉnh sửa chỉ số
Ghi log thay đổi
Điều chỉnh chỉ số

Các rule quan trọng cần thiết kế

1 meter chỉ thuộc:

1 Room
1 UtilityType

1 Room có thể có nhiều meter

Ví dụ

Electric meter
Water meter

Usage phải luôn >= 0

CurrentValue phải >= PreviousValue

MeterReading chỉ được ghi 1 lần mỗi kỳ.