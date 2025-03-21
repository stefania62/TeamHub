import axios from "axios";

export const API_BASE_URL = "https://localhost:7073/api";

const getAuthHeader = () => {
    const token = localStorage.getItem("token");
    if (!token) {
        return {};
    }
    return { Authorization: `Bearer ${token}` };
};

// Get all employees
export const getEmployees = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/admin/users`, {
            headers: getAuthHeader()
        });
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

// Create employee
export const createEmployee = async (employeeModel) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/admin/create-employee`,
            employeeModel,
            { headers: getAuthHeader() }
        );
        return response.data;
    } catch (error) {
        // Extracting the error message(s)
        const errorMessage = error.response?.data?.errors || "An unexpected error occurred.";

        // If the error contains specific field errors, format them for display
        let formattedError = "";
        if (error.response?.data?.errors) {
            formattedError = Object.values(error.response.data.errors)
                .flat()  
                .join(" ");
        } else {
            formattedError = errorMessage;
        }

        throw formattedError;
    }
};

// Update employee
export const updateEmployee = async (employeeModel) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/admin/update-user/${employeeModel.id}`,
            employeeModel,
            { headers: getAuthHeader() }
        );
        return response.data;
    } catch (error) {
        // Extracting the error message(s)
        const errorMessage = error.response?.data?.errors || "An unexpected error occurred.";

        let formattedError = "";
        if (error.response?.data?.errors) {
            formattedError = Object.values(error.response.data.errors)
                .flat() 
                .join(" ");
        } else {
            formattedError = errorMessage;
        }

        throw formattedError;
    }
};
// Delete employee
export const deleteEmployee = async (employeeId) => {
    try {
        await axios.delete(`${API_BASE_URL}/admin/delete-user/${employeeId}`, {
            headers: getAuthHeader(),
        });
        return { success: true };

    } catch (error) {
        throw error.response.data;
    }
};

// Create a new project
export const createProject = async (projectModel) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/projects`,
            projectModel, 
            {
                headers: getAuthHeader() 
            }
        );
        return response.data; 
    } catch (error) {
        throw error.response.data; 
    }
};

// Update project
export const updateProject = async (projectId, updatedData) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/projects/${projectId}`,
            updatedData,
            { headers: getAuthHeader() }
        );
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

// Delete a project
export const deleteProject = async (projectId) => {
    try {
        await axios.delete(`${API_BASE_URL}/projects/${projectId}`, {
            headers: getAuthHeader() 
        });
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

// Get all projects
export const getProjects = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/projects`, {
            headers: getAuthHeader() 
        });
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

// Assign an employee to a project
export const assignEmployeeToProject = async (projectId, employeeId) => {
    try {
        await axios.post(
            `${API_BASE_URL}/projects/${projectId}/assign`,
            { employeeId },
            {
                headers: getAuthHeader() 
            }
        );
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

// Remove an employee from a project
export const removeEmployeeFromProject = async (projectId, employeeId) => {
    try {
        await axios.delete(
            `${API_BASE_URL}/projects/${projectId}/remove/${employeeId}`,
            {
                headers: getAuthHeader() 
            }
        );
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

// Create a new task
export const createTask = async (title, projectId, assignedUserId) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/tasks`,
            { title, projectId, assignedUserId },
            {
                headers: getAuthHeader() 
            }
        );
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

// Delete a task
export const deleteTask = async (taskId) => {
    try {
        await axios.delete(`${API_BASE_URL}/tasks/${taskId}`, {
            headers: getAuthHeader() 
        });
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

// Mark a task as completed
export const completeTask = async (taskId) => {
    try {
        await axios.put(
            `${API_BASE_URL}/tasks/${taskId}/complete`,
            {},
            {
                headers: getAuthHeader() 
            }
        );
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};

// Get tasks for the authenticated user
export const getUserTasks = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/tasks`, {
            headers: getAuthHeader() 
        });
        return response.data;
    } catch (error) {
        throw error.response.data;
    }
};

// Assign a task to an employee
export const assignTask = async (taskId, employeeId) => {
    try {
        await axios.put(
            `${API_BASE_URL}/tasks/${taskId}/assign`,
            { employeeId },
            {
                headers: getAuthHeader() 
            }
        );
        return { success: true };
    } catch (error) {
        throw error.response.data;
    }
};
