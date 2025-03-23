import axios from "axios";

export const API_BASE_URL = "https://localhost:7073/api";

// Token
const getAuthHeader = () => {
    const token = localStorage.getItem("token");
    if (!token) {
        return {};
    }
    return { Authorization: `Bearer ${token}` };
};

// Format errors
const formatErrors = (error, fallbackMessage) => {
    const backendMessage = error?.response?.data?.message;
    const validationErrors = error?.response?.data?.errors;

    let formattedErrors = [];

    if (validationErrors) {
        formattedErrors = Object.values(validationErrors).flat();
    } else if (backendMessage) {
        formattedErrors = [backendMessage];
    } else {
        formattedErrors = [fallbackMessage];
    }

    return formattedErrors;
};

//#region Employee

// Get all employees
export const getEmployees = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/admin/users`, {
            headers: getAuthHeader()
        });
        return response.data;
    } catch (error) {
        const backendMessage = error?.response?.data?.message;
        const validationErrors = error?.response?.data?.errors;

        let formattedErrors = [];

        if (validationErrors) {
            formattedErrors = Object.values(validationErrors).flat();
        } else if (backendMessage) {
            formattedErrors = [backendMessage];
        } else {
            formattedErrors = ["An unexpected error occurred while fetching employees."];
        }

        throw formattedErrors;
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
        const backendMessage = error?.response?.data?.message;
        const validationErrors = error?.response?.data?.errors;

        let formattedErrors = [];

        if (validationErrors) {
            formattedErrors = Object.values(validationErrors).flat();
        } else if (backendMessage) {
            formattedErrors = [backendMessage];
        } else {
            formattedErrors = ["An unexpected error occurred while creating the employee."];
        }

        throw formattedErrors;
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
        const backendMessage = error?.response?.data?.message;
        const validationErrors = error?.response?.data?.errors;

        let formattedErrors = [];

        if (validationErrors) {
            formattedErrors = Object.values(validationErrors).flat();
        } else if (backendMessage) {
            formattedErrors = [backendMessage];
        } else {
            formattedErrors = ["An unexpected error occurred while updating the employee."];
        }

        throw formattedErrors;
    }
};

// Delete employee
export const deleteEmployee = async (employeeId) => {
    try {
        const response = await axios.delete(`${API_BASE_URL}/admin/delete-user/${employeeId}`, {
            headers: getAuthHeader(),
        });
        return response.data;
    } catch (error) {
        const backendMessage = error?.response?.data?.message;
        const validationErrors = error?.response?.data?.errors;

        let formattedErrors = [];

        if (validationErrors) {
            formattedErrors = Object.values(validationErrors).flat();
        } else if (backendMessage) {
            formattedErrors = [backendMessage];
        } else {
            formattedErrors = ["An unexpected error occurred while deleting the employee."];
        }

        throw formattedErrors;
    }
};

//#endregion

//#region User Profile

// Get current user profile
export const getCurrentUserProfile = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/user/profile`, {
            headers: getAuthHeader()
        });
        return response.data;
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while fetching the profile.");
    }
};

// Update user profile
export const updateProfile = async (profileModel) => {
    try {
        const formData = new FormData();
        formData.append("FullName", profileModel.fullName);
        formData.append("Username", profileModel.username);
        formData.append("Email", profileModel.email);
        if (profileModel.password) {
            formData.append("Password", profileModel.password);
        }
        profileModel.roles?.forEach(role => formData.append("Roles", role)); // if roles is array

        if (profileModel.profilePicture) {
            formData.append("ProfilePicture", profileModel.profilePicture);
        }

        const response = await axios.put(
            `${API_BASE_URL}/user/update-profile`,
            formData,
            {
                headers: {
                    ...getAuthHeader(),
                    "Content-Type": "multipart/form-data"
                }
            }
        );

        return response.data;
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while updating the profile.");
    }
};

//#endregion

//#region Project

// Get all projects
export const getProjects = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/projects`, {
            headers: getAuthHeader()
        });
        return response.data;
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while fetching projects.");
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
        throw formatErrors(error, "An unexpected error occurred while creating the project.");
    }
};

// Update project
export const updateProject = async (projectModel) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/projects/${projectModel.id}`,
            projectModel,
            { headers: getAuthHeader() }
        );
        return response.data;
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while updating the project.");
    }
};

// Assign an employee to a project
export const assignEmployeeToProject = async (projectId, employeeId) => {
    try {
        await axios.post(
            `${API_BASE_URL}/projects/${projectId}/assign/${employeeId}`,
            null,
            {
                headers: getAuthHeader()
            }
        );
        return { success: true };
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while assigning the employee to the project.");
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
        throw formatErrors(error, "An unexpected error occurred while removing the employee from the project.");
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
        throw formatErrors(error, "An unexpected error occurred while deleting the project.");
    }
};

//#endregion

//#region Task

// Get tasks 
export const getTasks = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/tasks`, {
            headers: getAuthHeader()
        });
        return response.data;
    } catch (error) {
        const backendMessage = error?.response?.data?.message;
        const validationErrors = error?.response?.data?.errors;

        let formattedErrors = [];

        if (validationErrors) {
            formattedErrors = Object.values(validationErrors).flat();
        } else if (backendMessage) {
            formattedErrors = [backendMessage];
        } else {
            formattedErrors = ["An unexpected error occurred while fetching tasks."];
        }

        throw formattedErrors;
    }
};

// Create a new task
export const createTask = async (taskModel) => {
    try {
        const response = await axios.post(
            `${API_BASE_URL}/tasks`,
            taskModel,
            {
                headers: getAuthHeader()
            }
        );
        return response.data;
    } catch (error) {
        const backendMessage = error?.response?.data?.message;
        const validationErrors = error?.response?.data?.errors;

        let formattedErrors = [];

        if (validationErrors) {
            formattedErrors = Object.values(validationErrors).flat();
        } else if (backendMessage) {
            formattedErrors = [backendMessage];
        } else {
            formattedErrors = ["An unexpected error occurred while creating the task."];
        }

        throw formattedErrors;
    }
};

export const updateTask = async (taskModel) => {
    try {
        const response = await axios.put(
            `${API_BASE_URL}/tasks/${taskModel.id}`,
            taskModel,
            { headers: getAuthHeader() }
        );
        return response.data;
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while updating the project.");
    }
};

// Mark task as completed
export const completeTask = async (taskId) => {
    try {
        await axios.put(`${API_BASE_URL}/tasks/mark-complete/${taskId}`, null, {
            headers: getAuthHeader()
        });

        return { success: true };
    } catch (error) {
        const backendMessage = error?.response?.data?.message;
        throw [backendMessage || "An unexpected error occurred while marking the task as completed."];
    }
};

// Assign an employee to a task
export const assignEmployeeToTask = async (tasksId, employeeId) => {
    try {
        await axios.post(
            `${API_BASE_URL}/tasks/${tasksId}/assign/${employeeId}`,
            null,
            {
                headers: getAuthHeader()
            }
        );
        return { success: true };
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while assigning the employee to the task.");
    }
};

// Remove an employee from a project
export const removeEmployeeFromTask = async (taskId, employeeId) => {
    try {
        await axios.delete(
            `${API_BASE_URL}/tasks/${taskId}/remove/${employeeId}`,
            {
                headers: getAuthHeader()
            }
        );
        return { success: true };
    } catch (error) {
        throw formatErrors(error, "An unexpected error occurred while removing the employee from the task.");
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
        const backendMessage = error?.response?.data?.message;
        throw [backendMessage || "An unexpected error occurred while deleting the task."];
    }
};

//#endregion
