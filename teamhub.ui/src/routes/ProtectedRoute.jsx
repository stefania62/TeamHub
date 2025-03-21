import React from "react";
import { Navigate } from "react-router-dom";
import { getUserRole } from "../utils/auth";

const ProtectedRoute = ({ element, allowedRoles }) => {
    const role = getUserRole(); 

    if (!role || !allowedRoles.includes(role)) {
        return <Navigate to="/login" />;
    }

    return element;
};

export default ProtectedRoute;
