using EventContracts.Authorization.Permissions;
namespace EventContracts.Authorization.Definitions.PermissionDefinitions
{
    public static class PropertyPermissionDefinitions
    {
        public static IEnumerable<BasePermissionDefinition> Get()
        {
            // Branch Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.BranchViewId,
                Code = BranchPermissions.VIEW,
                Name = "View Branch",
                Group = "Branch",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.BranchCreateId,
                Code = BranchPermissions.CREATE,
                Name = "Create Branch",
                Group = "Branch",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.BranchUpdateId,
                Code = BranchPermissions.UPDATE,
                Name = "Update Branch",
                Group = "Branch",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.BranchDeleteId,
                Code = BranchPermissions.DELETE,
                Name = "Delete Branch",
                Group = "Branch",
                SortOrder = 4
            };

            // Amenity Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.AmenityViewId,
                Code = AmenityPermissions.VIEW,
                Name = "View Amenity",
                Group = "Amenity",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.AmenityCreateId,
                Code = AmenityPermissions.CREATE,
                Name = "Create Amenity",
                Group = "Amenity",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.AmenityUpdateId,
                Code = AmenityPermissions.UPDATE,
                Name = "Update Amenity",
                Group = "Amenity",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.AmenityDeleteId,
                Code = AmenityPermissions.DELETE,
                Name = "Delete Amenity",
                Group = "Amenity",
                SortOrder = 4
            };

            // Maintenace Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.MaintenanceViewId,
                Code = MaintenancePermissions.VIEW,
                Name = "View Maintenance",
                Group = "Maintenance",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.MaintenanceCreateId,
                Code = MaintenancePermissions.CREATE,
                Name = "Create Maintenance",
                Group = "Maintenance",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.MaintenanceUpdateId,
                Code = MaintenancePermissions.UPDATE,
                Name = "Update Maintenance",
                Group = "Maintenance",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.MaintenanceDeleteId,
                Code = MaintenancePermissions.DELETE,
                Name = "Delete Maintenance",
                Group = "Maintenance",
                SortOrder = 4
            };

            // Reservation Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.ReservationViewId,
                Code = ReservationPermissions.VIEW,
                Name = "View Reservation",
                Group = "Reservation",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ReservationCreateId,
                Code = ReservationPermissions.CREATE,
                Name = "Create Reservation",
                Group = "Reservation",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.ReservationCancelId,
                Code = ReservationPermissions.CANCEL,
                Name = "Cancel Reservation",
                Group = "Reservation",
                SortOrder = 3
            };

            // RoomFacility Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomFacilityViewId,
                Code = RoomFacilityPermissions.VIEW,
                Name = "View Room Facility",
                Group = "Room Facility",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomFacilityCreateId,
                Code = RoomFacilityPermissions.CREATE,
                Name = "Create Room Facility",
                Group = "Room Facility",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomFacilityUpdateId,
                Code = RoomFacilityPermissions.UPDATE,
                Name = "Update Room Facility",
                Group = "Room Facility",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomFacilityDeleteId,
                Code = RoomFacilityPermissions.DELETE,
                Name = "Delete Room Facility",
                Group = "Room Facility",
                SortOrder = 4
            };

            // RoomHistory Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomHistoryViewId,
                Code = RoomHistoryPermissions.VIEW,
                Name = "View Room History",
                Group = "Room History",
                SortOrder = 1
            };

            // Room Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomViewId,
                Code = RoomPermissions.VIEW,
                Name = "View Room",
                Group = "Room",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomCreateId,
                Code = RoomPermissions.CREATE,
                Name = "Create Room",
                Group = "Room",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomUpdateId,
                Code = RoomPermissions.UPDATE,
                Name = "Update Room",
                Group = "Room",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomDeleteId,
                Code = RoomPermissions.DELETE,
                Name = "Delete Room",
                Group = "Room",
                SortOrder = 4
            };

            // RoomStatus Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomStatusLogViewId,
                Code = RoomStatusLogPermissions.VIEW,
                Name = "View Room Status",
                Group = "Room Status",
                SortOrder = 1
            };

            // RoomType Permissions
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomTypeViewId,
                Code = RoomTypePermissions.VIEW,
                Name = "View Room Type",
                Group = "Room Type",
                SortOrder = 1
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomTypeCreateId,
                Code = RoomTypePermissions.CREATE,
                Name = "Create Room Type",
                Group = "Room Type",
                SortOrder = 2
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomTypeUpdateId,
                Code = RoomTypePermissions.UPDATE,
                Name = "Update Room Type",
                Group = "Room Type",
                SortOrder = 3
            };
            yield return new BasePermissionDefinition
            {
                Id = DefId.RoomTypeDeleteId,
                Code = RoomTypePermissions.DELETE,
                Name = "Delete Room Type",
                Group = "Room Type",
                SortOrder = 4
            };
        }
    }
}
