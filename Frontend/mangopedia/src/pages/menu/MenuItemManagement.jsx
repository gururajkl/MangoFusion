import { useState } from "react";
import MenuItemModal from "../../components/menuItem/MenuItemModal.jsx";
import MenuItemTable from "../../components/menuItem/MenuItemTabel";
import { useGetMenuItemsQuery } from "../../components/store/api/menuItemsApi.js";

export default function MenuItemManagement() {
  const { data: menuItems = [], isLoading, error } = useGetMenuItemsQuery();

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

  const handleFormSubmit = (fromData) => {
    try {
      setIsSubmitting(true);
      console.log("Form submitted with data:", fromData);
    } catch (error) {
      console.error(error);
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleCloseShowModal = () => {
    setShowModal(false);
    setFormData({
      name: "",
      category: "",
      description: "",
      price: "",
      specialTag: "",
      image: null,
    });
  };

  const handleInputChange = (e) => {
    const { name, value, files } = e.target;

    if (name === "image") {
      setFormData((prevData) => ({ ...prevData, [name]: files[0] }));
    } else {
      setFormData((prevData) => ({ ...prevData, [name]: value }));
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
