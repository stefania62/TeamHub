import React, { useEffect, useState } from "react";
import {
    getEmployees,
    createEmployee,
    updateEmployee,
    deleteEmployee,
    getProjects,
    createProject,
    updateProject,
    assignEmployeeToProject,
    removeEmployeeFromProject,
    deleteProject,
    getTasks,
    createTask,
    updateTask,
    completeTask,
    assignEmployeeToTask,
    removeEmployeeFromTask,
    deleteTask,
} from "../api";

const AdminDashboard = () => {
    const [employees, setEmployees] = useState([]);
    const [newEmployeeRole, setNewEmployeeRole] = useState('');
    const [newEmployeeFullname, setNewEmployeeFullname] = useState('');
    const [newEmployeeEmail, setNewEmployeeEmail] = useState('');
    const [newEmployeeUsername, setNewEmployeeUsername] = useState('');
    const [newEmployeePassword, setNewEmployeePassword] = useState('');
    const [editEmployeeRole, setEditEmployeeRole] = useState('');
    const [editEmployeeUsername, setEditEmployeeUsername] = useState('');
    const [editEmployeeEmail, setEditEmployeeEmail] = useState('');
    const [editEmployeeFullname, setEditEmployeeFullname] = useState('');
    const [editEmployeeId, setEditEmployeeId] = useState(null);
    const [selectedEmployee, setSelectedEmployee] = useState("");
    const [projects, setProjects] = useState([]);
    const [newProject, setNewProject] = useState("");
    const [editProjectId, setEditProjectId] = useState("");
    const [editProjectTitle, seteditProjectTitle] = useState("");
    const [editProjectDescription, setEditProjectDescription] = useState("");
    const [selectedEmployeeForProject, setselectedEmployeeForProject] = useState("");
    const [showCreateProjectForm, setShowCreateProjectForm] = useState(false);
    const [selectedProject, setSelectedProject] = useState("");
    const [selectedAssignProject, setSelectedAssignProject] = useState("");
    const [tasks, setTasks] = useState([]);
    const [editTaskId, setEditTaskId] = useState("");
    const [editTaskTitle, setEditTaskTitle] = useState("");
    const [editTaskDescription, setEditTaskDescription] = useState("");
    const [editTaskProjectId, setEditTaskProjectId] = useState("");
    const [editTaskIsCompleted, setEditTaskIsCompleted] = useState("");
    const [selectedEmployeeForTask, setselectedEmployeeForTask] = useState("");
    const [selectedTaskEmployee, setSelectedTaskEmployee] = useState("");
    const [selectedTask, setSelectedTask] = useState("");
    const [newDescription, setNewDescription] = useState("");
    const [newTaskTitle, setNewTaskTitle] = useState("");
    const [newTaskDescription, setNewTaskDescription] = useState("");
    const [activeTab, setActiveTab] = useState("welcome");
    const [showCreateForm, setShowCreateForm] = useState(false);
    const [showCreateTaskForm, setShowCreateTaskForm] = useState(false);
    const [errors, setErrors] = useState([]);
    const [success, setSuccess] = useState("");

    useEffect(() => {
        loadProjects();
        loadTasks();
        loadEmployees();
    }, []);


    useEffect(() => {
        if (errors.length > 0) {
            const timer = setTimeout(() => {
                setErrors([]);
            }, 4000);

            return () => clearTimeout(timer);
        }
    }, [errors]);

    useEffect(() => {
        if (success.length > 0) {
            const timer = setTimeout(() => {
                setSuccess("");
            }, 4000);

            return () => clearTimeout(timer);
        }
    }, [success]);

    //#region Employee

    // Get employees
    const loadEmployees = async () => {
        try {
            const data = await getEmployees();
            setEmployees(data);
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages); 
        }
    };

    // Create employee
    const handleCreateEmployee = async () => {
        if (!newEmployeeEmail || !newEmployeeFullname) {
            setErrors(["Employee name, email, and full name are required."]);
            return;
        }
        const employeeModel = {
            username: newEmployeeUsername,
            email: newEmployeeEmail,
            fullName: newEmployeeFullname,
            password: newEmployeePassword,
            virtualPath: "",
            roles: ["Employee"]
        };

        try {
            await createEmployee(employeeModel);
            setNewEmployeeEmail('');
            setNewEmployeeFullname('');
            setNewEmployeePassword('');
            setNewEmployeeUsername('');
            setNewEmployeeRole('');
            setErrors([]);
            loadEmployees();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Edit employee
    const handleEditEmployee = (employee) => {
        setEditEmployeeId(employee.id);
        setEditEmployeeEmail(employee.email);
        setEditEmployeeFullname(employee.fullName);
        setEditEmployeeRole(employee.role);
        setEditEmployeeUsername(employee.username);
    };

    const handleUpdateEmployee = async () => {
        if (!editEmployeeEmail || !editEmployeeFullname) {
            setErrors(["Employee name, email, and full name are required."]);
            return;
        }

        const employeeModel = {
            id: editEmployeeId,
            email: editEmployeeEmail,
            fullName: editEmployeeFullname,
            username: editEmployeeUsername,
            virtualPath: "",
            roles: ["Employee"],
        };

        try {
            await updateEmployee(employeeModel);
            setEditEmployeeId(null);
            setEditEmployeeFullname("");
            setEditEmployeeUsername("");
            setEditEmployeeEmail("");
            setErrors([]);
            loadEmployees();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Delete employee
    const handleDeleteEmployee = async (employeeId) => {
        try {
            await deleteEmployee(employeeId);
            loadEmployees();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Assign employee to project
    const handleAssignEmployee = async () => {
        if (!selectedEmployee || !selectedProject) {
            setErrors(["Please select both employee and project."]);
            return;
        }

        try {
            await assignEmployeeToProject(selectedProject, selectedEmployee);
            setSuccess("Assigned successfully!");
            loadProjects();
            loadEmployees();
            loadTasks();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Remove employee from project
    const handleRemoveEmployee = async () => {
        if (!selectedEmployee || !selectedProject) {
            setErrors(["Please select both employee and project."]);
            return;
        }

        try {
            await removeEmployeeFromProject(selectedProject, selectedEmployee);
            setSuccess("Removed successfully!");
            loadProjects();
            loadTasks();
            loadEmployees();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Assign employee to task
    const handleAssignEmployeeToTask = async () => {
        if (!selectedTaskEmployee || !selectedTask) {
            setErrors(["Please select both employee and task."]);
            return;
        }

        try {
            await assignEmployeeToTask(selectedTask, selectedTaskEmployee);
            setSuccess("Assigned successfully!");
            loadTasks();
            loadEmployees();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Remove employee from task
    const handleRemoveEmployeeFromTask = async () => {
        if (!selectedTaskEmployee || !selectedTask) {
            setErrors(["Please select both employee and task."]);
            return;
        }

        try {
            await removeEmployeeFromTask(selectedTask, selectedTaskEmployee);
            setSuccess("Removed successfully!");
            loadTasks();
            loadEmployees();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    //#endregion

    //#region Project

    // Get projects
    const loadProjects = async () => {
        try {
            const data = await getProjects();
            setProjects(data);
            setErrors([]); // Clear any previous errors
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages); // expects array
        }
    };

    // Create project
    const handleCreateProject = async () => {
        if (!newProject) {
            setErrors(["Project title is required."]);
            return;
        }

        const projectModel = {
            title: newProject,
            description: newDescription || "",
        };

        try {
            await createProject(projectModel);
            setNewProject("");
            setNewDescription("");
            setErrors([]); // Clear any previous errors
            loadProjects();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Edit project
    const handleEditProject = (project) => {
        setEditProjectId(project.id);
        setEditProjectTitle(project.title);
        setEditProjectDescription(project.description);
    };

    const handleUpdateProject = async () => {
        if (!editProjectTitle) {
            setErrors(["Project title is required."]);
            return;
        }

        const projectModel = {
            id: editProjectId,
            title: editProjectTitle,
            description: editProjectDescription || "",
        };

        try {
            await updateProject(projectModel);
            setEditProjectId("");
            seteditProjectTitle("");
            setEditProjectDescription("");
            setErrors([]);
            loadProjects();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Delete project
    const handleDeleteProject = async (projectId) => {
        try {
            await deleteProject(projectId);
            setErrors([]);
            loadProjects();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    //#endregion

    //#region Task

    // Get tasks
    const loadTasks = async () => {
        try {
            const data = await getTasks();
            setTasks(data);
            setErrors([]);
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Create tasks
    const handleCreateTask = async () => {
        if (!newTaskTitle || !selectedProject) {
            setErrors(["Task title and project are required."]);
            return;
        }

        const taskModel = {
            title: newTaskTitle,
            projectId: selectedProject,
            isCompleted: false,
            description: newTaskDescription,
            projectTitle: "",
            assignedUserId: null
        };

        try {
            await createTask(taskModel);
            setNewTaskTitle("");
            setNewTaskDescription("");
            setErrors([]);
            loadTasks();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Edit tasks
    const handleEditTask = (task) => {
        setEditTaskId(task.id);
        setEditTaskTitle(task.title);
        setEditTaskDescription(task.description);
        setEditTaskProjectId(task.projectId);
        setEditTaskIsCompleted(task.isCompleted);
    };

    const handleUpdateTask = async () => {
        if (!editTaskTitle || !editTaskProjectId) {
            setErrors(["Task title and project are required."]);
            return;
        }

        const taskModel = {
            id: editTaskId,
            title: editTaskTitle,
            description: editTaskDescription,
            projectId: editTaskProjectId,
            isCompleted: editTaskIsCompleted,
            projectTitle: "",
            assignedUserId: null
        };

        try {
            await updateTask(taskModel);
            setEditTaskId("");
            setEditTaskTitle("");
            setEditTaskDescription("");
            setEditTaskProjectId("");
            setErrors([]);
            loadTasks();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Delete tasks
    const handleDeleteTask = async (taskId) => {
        try {
            await deleteTask(taskId);
            setErrors([]);
            loadTasks();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // Mark as completed
    const handleMarkTaskCompleted = async (taskId) => {
        try {
            await completeTask(taskId);
            setSuccess("Task marked as completed.");
            setErrors([]);
            loadTasks();
        } catch (errorMessages) {
            console.log("Formatted errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    //#endregion

    //#region Tab Management

    // Tab Content
    const renderTabContent = () => {
        switch (activeTab) {
            case "welcome":
                return renderWelcomeTab();
            case "employees":
                return renderEmployeeManagement();
            case "projects":
                return renderProjectManagement();
            case "tasks":
                return renderTaskManagement();
            case "assignments":
                return renderAssignmentTab();
            default:
                return null;
        }
    };

    const renderWelcomeTab = () => (
        <div className="pt-3">
            <div className="p-5 rounded-4 shadow-lg bg-white border text-center">
                <h1 className="fw-bold mb-3">Welcome, Administrator!</h1>
                <p className="lead" style={{ color: '#223a47' }}>
                    This is your control center. Use the tabs above to manage employees, projects, and tasks with ease.
                </p>
                <img
                    src="https://cdn-icons-png.flaticon.com/512/942/942748.png"
                    alt="Admin"
                    style={{ width: "100px", marginTop: "20px", opacity: 0.85 }}
                />
            </div>
        </div>
    );

    // Employee Management
    const renderEmployeeManagement = () => (
        <div className="container pt-3">
            <div className="p-5 rounded-1 shadow-lg bg-white border">

                {/* Heading */}
                <h2 className="mb-4 fw-bold">Employee Management</h2>
                <p className="text-muted mb-1 ">
                    Manage employee profiles, roles, and access within your organization.
                </p>

                {/* Error Display */}
                {errors.length > 0 && (
                    <div className="alert alert-danger rounded-3">
                        <ul className="mb-0 ps-3">
                            {errors.map((err, index) => (
                                <li key={index} className="small">
                                    {err}
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                <div className="d-flex justify-content-end mb-4">
                    <button
                        className={`btn ${showCreateForm ? "btn-outline-danger" : "btn-outline-success"}`}
                        onClick={() => setShowCreateForm((prev) => !prev)}
                    >
                        {showCreateForm ? "Cancel" : "Add New Employee"}
                    </button>
                </div>


                {(showCreateForm || editEmployeeId) && (
                    <div className="mb-5">
                        {/* Create form */}
                        {showCreateForm && (
                            <div className="card p-4 border shadow-sm bg-light rounded-4 mb-4">
                                <h5 className="text-dark mb-3">Enter Employee Details</h5>
                                <div className="row g-3 align-items-center">
                                    <div className="col-md-2">
                                        <label className="form-label">Full Name</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            placeholder="e.g. John Doe"
                                            value={newEmployeeFullname}
                                            onChange={(e) => setNewEmployeeFullname(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-3">
                                        <label className="form-label">Email</label>
                                        <input
                                            type="email"
                                            className="form-control"
                                            placeholder="e.g. john.doe@company.com"
                                            value={newEmployeeEmail}
                                            onChange={(e) => setNewEmployeeEmail(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-2">
                                        <label className="form-label">Username</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            placeholder="e.g. johndoe"
                                            value={newEmployeeUsername}
                                            onChange={(e) => setNewEmployeeUsername(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-2">
                                        <label className="form-label">Password</label>
                                        <input
                                            type="password"
                                            className="form-control"
                                            placeholder="******"
                                            value={newEmployeePassword}
                                            onChange={(e) => setNewEmployeePassword(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-2">
                                        <label className="form-label">Role</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value="Employee"
                                            disabled
                                        />
                                    </div>
                                    <div className="col-md-1 d-grid">
                                        <label className="form-label invisible">Submit</label>
                                        <button className="btn btn-primary" onClick={handleCreateEmployee}>
                                            Create
                                        </button>
                                    </div>
                                </div>
                            </div>
                        )}

                        {/* Edit form */}
                        {editEmployeeId && (
                            <div className="p-4 bg-light border rounded-3">
                                <div className="d-flex justify-content-between align-items-center mb-3">
                                    <h5 className="text-dark mb-3"> Edit Employee</h5>

                                </div>
                                <div className="row g-3 align-items-center">
                                    <div className="col-md-2">
                                        <label className="form-label">Full Name</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={editEmployeeFullname}
                                            onChange={(e) => setEditEmployeeFullname(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-3">
                                        <label className="form-label">Email</label>
                                        <input
                                            type="email"
                                            className="form-control"
                                            value={editEmployeeEmail}
                                            onChange={(e) => setEditEmployeeEmail(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-2">
                                        <label className="form-label">Username</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value={editEmployeeUsername}
                                            onChange={(e) => setEditEmployeeUsername(e.target.value)}
                                        />
                                    </div>
                                    <div className="col-md-2">
                                        <label className="form-label">Role</label>
                                        <input
                                            type="text"
                                            className="form-control"
                                            value="Employee"
                                            disabled
                                        />
                                    </div>
                                    <div className="col-md-3 d-flex gap-2 align-items-end align-self-end mb-1">
                                        <button
                                            className="btn btn btn-success btn-sm"
                                            onClick={handleUpdateEmployee}
                                        >
                                            Update
                                        </button>
                                        <button
                                            className="btn btn btn-warning btn-sm"
                                            onClick={() => setEditEmployeeId(null)}
                                        >
                                            Cancel Edit
                                        </button>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
                )}

                {/* Employees Table */}
                <div className="card shadow-sm border-0">
                    <div className="card-body">
                        <h4 className="card-title text-dark mb-4">All employees</h4>
                        <div className="table-responsive">
                            <table className="table align-middle table-hover table-bordered">
                                <thead className="table-light">
                                    <tr>
                                        <th>Full Name</th>
                                        <th>Email</th>
                                        <th>Username</th>
                                        <th>Role</th>
                                        <th className="text-center">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {employees.map((employee) => (
                                        <tr key={employee.id}>
                                            <td>{employee.fullName}</td>
                                            <td>{employee.email}</td>
                                            <td>{employee.username}</td>
                                            <td>{employee.roles}</td>
                                            <td className="text-center ">
                                                {!employee.roles.includes("Administrator") && (
                                                    <>
                                                        <button
                                                            className="btn btn-outline-success btn-sm me-2"
                                                            onClick={() => handleEditEmployee(employee)}
                                                        >
                                                            Edit
                                                        </button>
                                                        <button
                                                            className="btn btn-outline-danger btn-sm"
                                                            onClick={() => handleDeleteEmployee(employee.id)}
                                                        >
                                                            Delete
                                                        </button>
                                                    </>
                                                )}
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    );

    // Project Management
    const renderProjectManagement = () => (
        <div className="container pt-3">
            <div className="p-5 rounded-1 shadow-lg bg-white border">

                {/* Heading */}
                <h2 className="mb-4 fw-bold">Project Management</h2>
                <p className="text-muted mb-1">
                    Manage projects and track their progress across your organization.
                </p>

                {/* Error Display */}
                {errors?.length > 0 && (
                    <div className="alert alert-danger rounded-3">
                        <ul className="mb-0 ps-3">
                            {errors.map((err, index) => (
                                <li key={index} className="small">
                                    {err}
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                {/* Header + toggle button */}
                <div className="d-flex justify-content-end mb-4">
                    <button
                        className={`btn ${showCreateProjectForm ? "btn-outline-danger" : "btn-outline-success"}`}
                        onClick={() => setShowCreateProjectForm((prev) => !prev)}
                    >
                        {showCreateProjectForm ? "Cancel" : "Add New Project"}
                    </button>
                </div>

                {/* Create project form */}
                {showCreateProjectForm && (
                    <div className="card p-4 border shadow-sm bg-light rounded-4 mb-4">
                        <h5 className="text-dark mb-3">Enter Project Details</h5>
                        <div className="row g-3 align-items-center">
                            <div className="col-md-4">
                                <label className="form-label">Project Title</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. Internal CRM"
                                    value={newProject}
                                    onChange={(e) => setNewProject(e.target.value)}
                                />
                            </div>
                            <div className="col-md-6">
                                <label className="form-label">Description</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. A tool to manage customer relationships"
                                    value={newDescription}
                                    onChange={(e) => setNewDescription(e.target.value)}
                                />
                            </div>
                            <div className="col-md-2 d-grid">
                                <label className="form-label invisible">Submit</label>
                                <button
                                    className="btn btn-primary"
                                    onClick={handleCreateProject}
                                >
                                    Create
                                </button>
                            </div>
                        </div>
                    </div>
                )}

                {/* Edit project form */ }
                {editProjectId && (
                    <div className="card p-4 border shadow-sm bg-light rounded-4 mb-4">
                        <h5 className="text-dark mb-3">Edit Project Details</h5>
                        <div className="row g-3 align-items-center">
                            <div className="col-md-4">
                                <label className="form-label">Project Title</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. Internal CRM"
                                    value={editProjectTitle}
                                    onChange={(e) => seteditProjectTitle(e.target.value)}
                                />
                            </div>
                            <div className="col-md-5">
                                <label className="form-label">Description</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. A tool to manage customer relationships"
                                    value={editProjectDescription}
                                    onChange={(e) => setEditProjectDescription(e.target.value)}
                                />
                            </div>
                            <div className="col-md-3 d-flex gap-2 align-items-end align-self-end mb-1">
                                <button
                                    className="btn btn-primary btn-sm"
                                    onClick={handleUpdateProject}
                                >
                                    Update
                                </button>
                                <button
                                    className="btn btn btn-warning btn-sm"
                                    onClick={() => setEditProjectId(null)}
                                >
                                    Cancel
                                </button>
                            </div>
                        </div>
                    </div>
                )}
                {/* Projects Table */}
                <div className="card shadow-sm border-0">
                    <div className="card-body">
                        <h4 className="card-title text-dark mb-4">All Projects</h4>
                        <div className="table-responsive">
                            <table className="table align-middle table-hover table-bordered">
                                <thead className="table-light">
                                    <tr>
                                        <th>Project Title</th>
                                        <th>Description</th>
                                        <th className="text-center">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {projects.map((project) => (
                                        <tr key={project.id}>
                                            <td>{project.title}</td>
                                            <td>{project.description}</td>
                                            <td className="text-center">
                                                <button
                                                    className="btn btn-outline-success btn-sm me-2"
                                                    onClick={() => handleEditProject(project)}
                                                >
                                                    Edit
                                                </button>
                                                <button
                                                    className="btn btn-outline-danger btn-sm"
                                                    onClick={() => handleDeleteProject(project.id)}
                                                >
                                                    Delete
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    );

    // Task Management
    const renderTaskManagement = () => (
        <div className="container pt-3">
            <div className="p-5 rounded-1 shadow-lg bg-white border">

                {/* Heading */}
                <h2 className="mb-4 fw-bold">Task Management</h2>
                <p className="text-muted mb-1">
                    Create and manage tasks associated with your projects.
                </p>
                {success && (
                    <div className="alert alert-success rounded-3">
                        <span className="small">{success}</span>
                    </div>
                )}

                {/* Error Display */}
                {errors?.length > 0 && (
                    <div className="alert alert-danger rounded-3">
                        <ul className="mb-0 ps-3">
                            {errors.map((err, index) => (
                                <li key={index} className="small">
                                    {err}
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                {/* Header & toggle button*/}
                <div className="d-flex justify-content-end mb-4">
                    <button
                        className={`btn ${showCreateTaskForm ? "btn-outline-danger" : "btn-outline-success"}`}
                        onClick={() => setShowCreateTaskForm((prev) => !prev)}
                    >
                        {showCreateTaskForm ? "Cancel" : "Add New Task"}
                    </button>
                </div>

                {/* Create task form */}
                {showCreateTaskForm && (
                    <div className="card p-4 border shadow-sm bg-light rounded-4 mb-4">
                        <h5 className="text-dark mb-3">Enter Task Details</h5>
                        <div className="row g-3 align-items-center">
                            <div className="col-md-3">
                                <label className="form-label">Task Name</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. Initial Setup"
                                    value={newTaskTitle}
                                    onChange={(e) => setNewTaskTitle(e.target.value)}
                                />
                            </div>
                            <div className="col-md-4">
                                <label className="form-label">Description</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. New dashboard"
                                    value={newTaskDescription}
                                    onChange={(e) => setNewTaskDescription(e.target.value)}
                                />
                            </div>
                            <div className="col-md-3">
                                <label className="form-label">Select Project</label>
                                <select
                                    className="form-select"
                                    value={selectedProject}
                                    onChange={(e) => setSelectedProject(e.target.value)}
                                >
                                    <option value="">Select Project</option>
                                    {projects.map((project) => (
                                        <option key={project.id} value={project.id}>
                                            {project.title}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="col-md-2 d-grid align-self-end">
                                <button
                                    className="btn btn-outline-success"
                                    onClick={handleCreateTask}
                                >
                                    Create
                                </button>
                            </div>
                        </div>
                    </div>
                )}
                {/* Edit task form */ }
                {editTaskId && (
                    <div className="card p-4 border shadow-sm bg-light rounded-4 mb-4">
                        <h5 className="text-dark mb-3">Edit Task Details</h5>
                        <div className="row g-3 align-items-center">
                            <div className="col-md-3">
                                <label className="form-label">Task Name</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. Bug Fix"
                                    value={editTaskTitle}
                                    onChange={(e) => setEditTaskTitle(e.target.value)}
                                />
                            </div>
                            <div className="col-md-4">
                                <label className="form-label">Description</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    placeholder="e.g. Fix login issue"
                                    value={editTaskDescription}
                                    onChange={(e) => setEditTaskDescription(e.target.value)}
                                />
                            </div>
                            <div className="col-md-2">
                                <label className="form-label">Project</label>
                                <select
                                    className="form-select"
                                    value={editTaskProjectId}
                                    onChange={(e) => setEditTaskProjectId(e.target.value)}
                                >
                                    <option value="">Select Project</option>
                                    {projects.map((project) => (
                                        <option key={project.id} value={project.id}>
                                            {project.title}
                                        </option>
                                    ))}
                                </select>
                            </div>
                            <div className="col-md-3 d-flex gap-2 align-items-end align-self-end mb-1">
                                <button
                                    className="btn btn-primary btn-sm"
                                    onClick={handleUpdateTask}
                                >
                                    Update
                                </button>
                                <button
                                    className="btn btn btn-warning btn-sm"
                                    onClick={() => setEditTaskId(null)}
                                >
                                    Cancel
                                </button>
                            </div>
                        </div>
                    </div>
                )}

                {/* Tasks Table */}
                <div className="card shadow-sm border-0">
                    <div className="card-body">
                        <h4 className="card-title text-dark mb-4">All Tasks</h4>
                        <div className="table-responsive">
                            <table className="table align-middle table-hover table-bordered">
                                <thead className="table-light">
                                    <tr>
                                        <th>Task Title</th>
                                        <th>Task Description</th>
                                        <th>Assigned Employee</th>
                                        <th>Assigned Project</th>
                                        <th>Status</th>
                                        <th className="text-center">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {tasks.map((task) => (
                                        <tr key={task.id}>
                                            <td>{task.title}</td>
                                            <td>{task.description}</td>
                                            <td>{task.assignedUserName || "Unassigned"}</td>
                                            <td>{task.projectTitle}</td>
                                            <td>
                                                {task.isCompleted === true ? (
                                                    <span className="badge bg-success">Completed</span>
                                                ) : (
                                                    <span className="badge bg-secondary">Pending</span>
                                                )}
                                            </td>
                                            <td className="text-center">
                                                <button
                                                    className="btn btn-outline-success btn-sm me-2"
                                                    onClick={() => handleMarkTaskCompleted(task.id)}
                                                    disabled={task.isCompleted}
                                                >
                                                    Mark as Completed
                                                </button>
                                                <button
                                                    className="btn btn-outline-primary btn-sm me-2"
                                                    onClick={() => handleEditTask(task)}
                                                >
                                                    Update
                                                </button>
                                                <button
                                                    className="btn btn-outline-danger btn-sm"
                                                    onClick={() => handleDeleteTask(task.id)}
                                                >
                                                    Delete
                                                </button>
                                            </td>

                                        </tr>
                                    ))
                                    }
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    );

    // Assignment Management
    const renderAssignmentTab = () => (
        <div className="container pt-3">
            <div className="p-5 rounded-1 shadow-lg bg-white border">

                {/* Heading */}
                <h2 className="mb-2 fw-bold">Assignments</h2>
                <p className="text-muted mb-5">
                    Assign or remove employees to/from projects and tasks.
                </p>

                {success && (
                    <div className="alert alert-success rounded-3">
                        <span className="small">{success}</span>
                    </div>
                )}

                {/* Error Display */}
                {errors?.length > 0 && (
                    <div className="alert alert-danger rounded-3">
                        <ul className="mb-0 ps-3">
                            {errors.map((err, index) => (
                                <li key={index} className="small">
                                    {err}
                                </li>
                            ))}
                        </ul>
                    </div>
                )}

                {/* Assign to Project */}
                <div className="mb-5">
                    <h5 className="text-dark mb-3">Assign Employee to Project</h5>
                    <div className="row g-3 align-items-center">
                        <div className="col-md-4">
                            <label className="form-label">Select Project</label>
                            <select
                                className="form-select"
                                value={selectedProject}
                                onChange={(e) => setSelectedProject(e.target.value)}
                            >
                                <option value="">-- Select Project --</option>
                                {projects.map((project) => (
                                    <option key={project.id} value={project.id}>
                                        {project.title}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="col-md-4">
                            <label className="form-label">Select Employee</label>
                            <select
                                className="form-select"
                                value={selectedEmployee}
                                onChange={(e) => setSelectedEmployee(e.target.value)}
                            >
                                <option value="">-- Select Employee --</option>
                                {employees.map((emp) => (
                                    <option key={emp.id} value={emp.id}>
                                        {emp.fullName} ({emp.email})
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="col-md-3 d-flex gap-2 align-items-end align-self-end mb-1">
                            <button className="btn btn-outline-success" onClick={handleAssignEmployee}>
                                ➕ Assign
                            </button>
                            <button className="btn btn-outline-danger" onClick={handleRemoveEmployee}>
                                ➖ Remove
                            </button>
                        </div>
                    </div>
                </div>

                {/* Assign to Task */}
                <div>
                    <h5 className="text-dark mb-3">Assign Employee to Task</h5>
                    <div className="row g-3 align-items-center">
                        <div className="col-md-4">
                            <label className="form-label">Select Task</label>
                            <select
                                className="form-select"
                                value={selectedTask}
                                onChange={(e) => setSelectedTask(e.target.value)}
                            >
                                <option value="">-- Select Task --</option>
                                {tasks.map((task) => (
                                    <option key={task.id} value={task.id}>
                                        {task.title}
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="col-md-4">
                            <label className="form-label">Select Employee</label>
                            <select
                                className="form-select"
                                value={selectedTaskEmployee}
                                onChange={(e) => setSelectedTaskEmployee(e.target.value)}
                            >
                                <option value="">-- Select Employee --</option>
                                {employees.map((emp) => (
                                    <option key={emp.id} value={emp.id}>
                                        {emp.fullName} ({emp.email})
                                    </option>
                                ))}
                            </select>
                        </div>
                        <div className="col-md-3 d-flex gap-2 align-items-end align-self-end mb-1">
                            <button className="btn btn-outline-success" onClick={handleAssignEmployeeToTask}>
                                ➕ Assign
                            </button>
                            <button className="btn btn-outline-danger" onClick={handleRemoveEmployeeFromTask}>
                                ➖ Remove
                            </button>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    );

    // Tab Buttons
    const handleTabChange = (tab) => {
        setActiveTab(tab);
    };

    //#endregion

    return (
        <div className="container mt-5">
            {/* Tabs Navigation */}
            <ul className="nav nav-tabs" role="tablist">
                <li className="nav-item">
                    <a
                        className={`nav-link fw-bold text-dark ${activeTab === "welcome" ? "active" : ""}`}
                        onClick={() => handleTabChange("welcome")}
                    >
                        Welcome
                    </a>
                </li>
                <li className="nav-item">
                    <a
                        className={`nav-link fw-bold text-dark ${activeTab === "employees" ? "active" : ""}`}
                        onClick={() => handleTabChange("employees")}
                    >
                        Employees
                    </a>
                </li>
                <li className="nav-item">
                    <a
                        className={`nav-link fw-bold text-dark ${activeTab === "projects" ? "active" : ""}`}
                        onClick={() => handleTabChange("projects")}
                    >
                        Projects
                    </a>
                </li>
                <li className="nav-item">
                    <a
                        className={`nav-link fw-bold text-dark ${activeTab === "tasks" ? "active" : ""}`}
                        onClick={() => handleTabChange("tasks")}
                    >
                        Tasks
                    </a>
                </li>
                <li className="nav-item">
                    <a
                        className={`nav-link fw-bold text-dark ${activeTab === "assignments" ? "active" : ""}`}
                        onClick={() => handleTabChange("assignments")}
                    >
                        Assignments
                    </a>
                </li>

            </ul>

            {/* Tab Content */}
            {renderTabContent()}
        </div>
    );
};

export default AdminDashboard;