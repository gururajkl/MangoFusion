import { useState } from "react";
import MenuItemModal from "../../components/menuItem/MenuItemModal.jsx";
import MenuItemTable from "../../components/menuItem/MenuItemTabel";
import {
  useCreateMenuItemMutation,
  useDeleteMenuItemMutation,
  useGetMenuItemsQuery,
  useUpdateMenuItemMutation,
} from "../../components/store/api/menuItemsApi.js";
import { toast } from "react-toastify";
import Swal from "sweetalert2";

export default function MenuItemManagement() {
  const {
    data: menuItems = [],
    isLoading,
    error,
    refetch,
  } = useGetMenuItemsQuery();
  const [createMenuItem] = useCreateMenuItemMutation();
  const [deleteMenuItem] = useDeleteMenuItemMutation();
  const [updateMenuItem] = useUpdateMenuItemMutation();

  console.log(menuItems);

  const [showModal, setShowModal] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const [formData, setFormData] = useState({
    name: "",
    category: "",
    description: "",
    price: "",
    specialTag: "",
    image: null,
  });

  const handleFormSubmit = async (fromData) => {
    try {
      setIsSubmitting(true);
      console.log("Form submitted with data:", fromData);

      // Construct FormData to send to the API.
      const formDataToSend = new FormData();
      formDataToSend.append("Name", formData.name);
      formDataToSend.append("Category", formData.category);
      formDataToSend.append("Description", formData.description);
      formDataToSend.append("Price", formData.price);
      formDataToSend.append("SpecialTag", formData.specialTag);

      if (formData.image) {
        formDataToSend.append("File", formData.image);
      }

      let result;
      result = await createMenuItem(formDataToSend);

      if (result.isSuccess !== false) {
        toast.success("Menu item created successfully!");
        // Calls query again.
        refetch();
      } else {
        toast.error("Failed to create menu item. Please try again.");
      }

      setShowModal(false);
      resetForm();
    } catch (error) {
      console.error(error);
      toast.error("Failed to create menu item. Please try again.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCloseShowModal = () => {
    setShowModal(false);
    resetForm();
  };

  const handleInputChange = (e) => {
    const { name, value, files } = e.target;

    if (name === "image") {
      setFormData((prevData) => ({ ...prevData, [name]: files[0] }));
    } else {
      setFormData((prevData) => ({ ...prevData, [name]: value }));
    }
  };

  const resetForm = () => {
    setFormData({
      name: "",
      category: "",
      description: "",
      price: "",
      specialTag: "",
      image: null,
    });
  };

  const handleDeleteMenuItem = async (itemId) => {
    const result = await Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!",
    });

    if (result.isConfirmed) {
      await deleteMenuItem(itemId);
      Swal.fire({
        title: "Deleted!",
        text: "Your menu item has been deleted.",
        icon: "success",
      });
    }
  };

  return (
    <div className="container-fluid p-4">
      <div className="row mb-4">
        <div className="col">
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <h2>Menu Item Management</h2>
              <p className="text-muted mb-0">
                Manage your restaurant's menu items
              </p>
            </div>
            <button
              onClick={() => setShowModal(true)}
              className="btn btn-primary"
            >
              <i className="bi bi-plus-circle me-2"></i>
              Add Menu Item
            </button>
          </div>
        </div>
      </div>
      <div className="row">
        <div className="col">
          <div className="card">
            <div className="card-body">
              <MenuItemTable
                onDelete={handleDeleteMenuItem}
                menuItems={menuItems}
                isLoading={isLoading}
                error={error}
              />
            </div>
          </div>
        </div>
      </div>
      {showModal && (
        <MenuItemModal
          onChange={handleInputChange}
          formData={formData}
          onSubmit={handleFormSubmit}
          onClose={handleCloseShowModal}
          isSubmitting={isSubmitting}
        />
      )}
    </div>
  );
}
