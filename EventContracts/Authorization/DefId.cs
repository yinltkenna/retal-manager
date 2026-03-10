namespace EventContracts.Authorization
{
    public static class DefId
    {
        // --- ROLES (Hệ thống vai trò) ---
        public static readonly Guid AdminId = Guid.Parse("79269784-9892-4984-933e-6369c0d95261");
        public static readonly Guid ManagerId = Guid.Parse("37812975-680c-4394-918d-6477e600d89c");
        public static readonly Guid UserId = Guid.Parse("8453304a-4952-45f4-8182-3e28038c356d");

        // --- USER MANAGEMENT (Quản lý người dùng) ---
        public static readonly Guid UserViewId = Guid.Parse("a8e96f8f-6ae8-4ffb-bb90-5a90786951d1");
        public static readonly Guid UserCreateId = Guid.Parse("c9a3c1a0-2d0a-4a88-bfa2-72ee4178fd3f");
        public static readonly Guid UserUpdateId = Guid.Parse("3f9c82c7-aa6e-4b34-9e35-971f2be1a378");
        public static readonly Guid UserDeleteId = Guid.Parse("7c5d8dc0-7169-4b2e-8d44-9a7bb9fcc0e8");
        public static readonly Guid UserLockId = Guid.Parse("e8c1e1b3-50ac-4361-a2f1-3736ca3d6a92");
        public static readonly Guid UserViewLogsId = Guid.Parse("1b5aeb2f-e5b6-41c4-9c6a-3a8a9acb0d3f");

        // --- BRANCH/BRAND (Chi nhánh) ---
        public static readonly Guid BranchViewId = Guid.Parse("5f93976f-6072-49c8-9712-4211603534a6");
        public static readonly Guid BranchCreateId = Guid.Parse("58d4a7f0-0899-4c5b-9d41-3375b4306381");
        public static readonly Guid BranchUpdateId = Guid.Parse("96d53be7-6b45-423f-b328-910f56a3949e");
        public static readonly Guid BranchDeleteId = Guid.Parse("80267756-3f11-4770-9884-63556708687a");

        // --- ROOM TYPE (Loại phòng) ---
        public static readonly Guid RoomTypeViewId = Guid.Parse("6a827083-d336-414f-9e79-58f700445f1b");
        public static readonly Guid RoomTypeCreateId = Guid.Parse("8a0c242c-297a-4c91-9566-1c090333d45f");
        public static readonly Guid RoomTypeUpdateId = Guid.Parse("4e6027c9-9430-4e94-879e-71624b4226cc");
        public static readonly Guid RoomTypeDeleteId = Guid.Parse("26d83095-2f88-466d-88f6-175510006798");

        // --- ROOM (Phòng thực tế) ---
        public static readonly Guid RoomViewId = Guid.Parse("9749a037-1229-4171-bc08-01186b168916");
        public static readonly Guid RoomCreateId = Guid.Parse("f9812999-523c-4034-9279-052264663529");
        public static readonly Guid RoomUpdateId = Guid.Parse("4a065666-412e-48a0-ba0e-660321351110");
        public static readonly Guid RoomDeleteId = Guid.Parse("cd225381-e23a-4933-911d-616954217158");

        // --- AMENITY & FACILITIES (Tiện ích) ---
        public static readonly Guid AmenityViewId = Guid.Parse("64295847-7977-463d-888a-215035987625");
        public static readonly Guid AmenityCreateId = Guid.Parse("80753691-8815-4303-9121-729918738676");
        public static readonly Guid AmenityUpdateId = Guid.Parse("59298246-8318-4720-bc2d-522606548549");
        public static readonly Guid AmenityDeleteId = Guid.Parse("09569033-6625-4670-b615-384405391210");

        public static readonly Guid RoomFacilityViewId = Guid.Parse("33678385-2638-4105-9502-364402636830");
        public static readonly Guid RoomFacilityCreateId = Guid.Parse("09903930-8115-4662-8706-037678121650");
        public static readonly Guid RoomFacilityUpdateId = Guid.Parse("98418080-6925-4554-9405-592659775310");
        public static readonly Guid RoomFacilityDeleteId = Guid.Parse("29272365-1544-4601-9252-879893414960");

        // --- OPERATIONS (Vận hành) ---
        public static readonly Guid ReservationViewId = Guid.Parse("61008681-3064-4458-9442-701334057884");
        public static readonly Guid ReservationCreateId = Guid.Parse("70634674-3255-4660-8488-212239472146");
        public static readonly Guid ReservationCancelId = Guid.Parse("55461159-4509-4082-9694-893504177432");

        public static readonly Guid MaintenanceViewId = Guid.Parse("56728092-2374-4537-8821-665123512340");
        public static readonly Guid MaintenanceCreateId = Guid.Parse("09812351-1234-4567-8901-223344556677");
        public static readonly Guid MaintenanceUpdateId = Guid.Parse("11223344-5566-7788-9900-aabbccddeeff");
        public static readonly Guid MaintenanceDeleteId = Guid.Parse("ffeeddcc-bbaa-0099-8877-665544332211");

        public static readonly Guid RoomHistoryViewId = Guid.Parse("99887766-5544-3322-1100-001122334455");

        public static readonly Guid RoomStatusLogViewId = Guid.Parse("a2a4b6a8-90ab-cdef-b234-5678a0abcdef");
    }
}