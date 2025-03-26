import React, { useEffect, useState } from "react";
import {
    getProjects,
    getTasks,
    createTask,
    updateTask,
    completeTask,
    assignEmployeeToTask,
    removeEmployeeFromTask,
    getEmployees,
    updateProfile,
    getCurrentUserProfile
} from "../api";

const EmployeeDashboard = () => {
    const [activeTab, setActiveTab] = useState("welcome");
    const [profile, setProfile] = useState({ fullName: "", email: "", username: "" });
    const [selectedFile, setSelectedFile] = useState(null);
    const [virtualPath, setVirtualPath] = useState("");
    const [projects, setProjects] = useState([]);
    const [tasks, setTasks] = useState([]);
    const [editTaskId, setEditTaskId] = useState("");
    const [editTaskTitle, setEditTaskTitle] = useState("");
    const [editTaskDescription, setEditTaskDescription] = useState("");
    const [editTaskProjectId, setEditTaskProjectId] = useState("");
    const [showCreateTaskForm, setShowCreateTaskForm] = useState(false);
    const [newTaskTitle, setNewTaskTitle] = useState("");
    const [newDescription, setNewDescription] = useState("");
    const [newTaskDescription, setNewTaskDescription] = useState("");
    const [selectedProject, setSelectedProject] = useState("");
    const [selectedTaskEmployee, setSelectedTaskEmployee] = useState("");
    const [selectedTask, setSelectedTask] = useState("");
    const [employees, setEmployees] = useState([]);
    const [newTask, setNewTask] = useState({ title: "", description: "", projectId: "", assignedUserId: "" });
    const [errors, setErrors] = useState([]);
    const [success, setSuccess] = useState("");

    useEffect(() => {
        loadProfile();
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

    // #region Profile

    // Load profile
    const loadProfile = async () => {
        const data = await getCurrentUserProfile();
        setProfile(data);
        setVirtualPath(data.imageVirtualPath);
    };

    // Load employees
    const loadEmployees = async () => {
        const data = await getEmployees();
        setEmployees(data);
    };

    // Update profile
    const handleProfileUpdate = async () => {
        setErrors([]);
        setSuccess("");

        const profileModel = {
            fullName: profile.fullName,
            username: profile.username,
            password: profile.password,
            email: profile.email,
            roles: profile.roles,
            imageVirtualPath: selectedFile
        };

        try {
            await updateProfile(profileModel);
            setSuccess("Profile updated successfully.");
            setSelectedFile(null);
            loadProfile();
        } catch (errorMessages) {
            console.log("Profile update errors:", errorMessages);
            setErrors(errorMessages);
        }
    };

    // File change
    const handleFileChange = (e) => {
        const file = e.target.files[0];
        if (file) {
            setSelectedFile(file);
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

    // #region Task

    // Load tasks
    const loadTasks = async () => {
        const data = await getTasks();
        setTasks(data);
    };

    // Load projects
    const loadProjects = async () => {
        const data = await getProjects();
        setProjects(data);
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
            setShowCreateTaskForm(false);
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

    // Complete task
    const handleCompleteTask = async (taskId) => {
        try {
            await completeTask(taskId);
            setSuccess("Task marked as completed");
            loadTasks();
        } catch {
            setErrors(["Could not mark task as completed"]);
        }
    };

    //#endregion

    return (
        <div className="container mt-5">
            <ul className="nav nav-tabs">
                <li className="nav-item">
                    <button className={`nav-link fw-bold text-dark ${activeTab === "welcome" ? "active" : ""}`} onClick={() => setActiveTab("welcome")}>Welcome</button>
                </li>
                <li className="nav-item">
                    <button className={`nav-link fw-bold text-dark ${activeTab === "profile" ? "active" : ""}`} onClick={() => setActiveTab("profile")}>My Profile</button>
                </li>
                <li className="nav-item">
                    <button className={`nav-link fw-bold text-dark ${activeTab === "tasks" ? "active" : ""}`} onClick={() => setActiveTab("tasks")}>Tasks</button>
                </li>
                <li className="nav-item">
                    <button className={`nav-link fw-bold text-dark ${activeTab === "assignment" ? "active" : ""}`} onClick={() => setActiveTab("assignment")}>Assignment</button>
                </li>
            </ul>

            {activeTab === "welcome" && (
                < div className="pt-3">
                    <div className="p-5 rounded-4 shadow-lg bg-white border text-center">
                        <h1 className="fw-bold mb-3">Welcome!</h1>
                        <p className="lead" style={{ color: '#223a47' }}>
                            This is your control center. Use the tabs above to manage your profile and tasks with ease.
                        </p>
                        <img
                            src="https://cdn-icons-png.flaticon.com/512/15757/15757011.png"
                            alt="Employee"
                            style={{ width: "100px", marginTop: "20px", opacity: 0.85 }}
                        />
                    </div>
                </div>
            )}

            {activeTab === "profile" && (
                <div className="p-4 bg-white shadow mt-3 rounded d-flex justify-content-between">
                    <div className="w-50">
                        <h3>
                            <img src="https://cdn-icons-png.flaticon.com/512/18827/18827925.png"
                                alt="User Icon"
                                width="30"
                                height="30"
                                style={{ marginRight: '10px', verticalAlign: 'middle' }}
                            />
                            Update Profile
                        </h3>
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
                        <input type="text" className="form-control my-2 mt-4 w-75" style={{ border: '1px solid #333', marginBottom: '1rem' }} value={profile.fullName} onChange={(e) => setProfile({ ...profile, fullName: e.target.value })} placeholder="Full Name" />
                        <input type="email" className="form-control my-2 w-75" style={{ border: '1px solid #333', marginBottom: '1rem' }} value={profile.email} onChange={(e) => setProfile({ ...profile, email: e.target.value })} placeholder="Email" />
                        <input type="text" className="form-control my-2 w-75" style={{ border: '1px solid #333', marginBottom: '1rem' }} value={profile.username} onChange={(e) => setProfile({ ...profile, username: e.target.value })} placeholder="Username" />
                        <input type="password" className="form-control my-2 w-75" style={{ border: '1px solid #333', marginBottom: '1rem' }} value={profile.password || ""} onChange={(e) => setProfile({ ...profile, password: e.target.value })} placeholder="New Password (optional)" />
                        <input type="text" className="form-control my-2 w-75" style={{ border: '1px solid #333', marginBottom: '1rem' }} value={`Role: ${profile.roles?.join(', ')}`} disabled placeholder="Roles" />
                        <input type="file" className="form-control my-2 w-75" style={{ border: '1px solid #333', marginBottom: '1rem' }} onChange={handleFileChange} />
                        <button className="btn btn-success w-75 mt-3" onClick={handleProfileUpdate}>Update</button>
                    </div>
                    <div className="d-flex flex-column align-items-start w-50 pt-2">
                        {profile.imageVirtualPath && (
                            <>
                                <img
                                    src={`http://localhost:5000${profile.imageVirtualPath}`}
                                    alt="Profile"
                                    className="img-thumbnail mt-5 ms-5"
                                    style={{ maxWidth: '400px', height: 'auto' }}
                                />

                            </>
                        )}
                    </div>
                </div>
            )}

            {activeTab === "tasks" && (
                <div className="p-4 bg-white shadow mt-3 rounded">
                    <h3 className="ms-3 mt-3">All Tasks</h3>
                    {/* Header & toggle button*/}
                    <div className="d-flex justify-content-end mb-4">
                        <button
                            className={`btn ${showCreateTaskForm ? "btn-outline-danger" : "btn-outline-success"}`}
                            onClick={() => setShowCreateTaskForm((prev) => !prev)}
                        >
                            {showCreateTaskForm ? "Cancel" : "Create"}
                        </button>
                    </div>

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

                    {/* Create task form */}
                    {showCreateTaskForm && (
                        <div className="card p-4 border shadow-sm bg-light rounded-4 mb-4">
                            <h5 className="text-dark mb-3">Enter Task Details</h5>
                            <div className="row g-3 align-items-center">
                                <div className="col-md-3">
                                    <label className="form-label">Task Title</label>
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

                    {/* Edit task form */}
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


                    <table className="table table-bordered mt-4">
                        <thead>
                            <tr>
                                <th>Title</th>
                                <th>Description</th>
                                <th>Project</th>
                                <th>Assigned to</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {tasks.map((task) => (
                                <tr key={task.id}>
                                    <td>{task.title}</td>
                                    <td>{task.description}</td>
                                    <td>{task.projectTitle}</td>
                                    <td>{task.assignedUserName}</td>
                                    <td>
                                        {task.isCompleted === true ? (
                                            <span className="badge bg-success">Completed</span>
                                        ) : (
                                            <span className="badge bg-secondary">Pending</span>
                                        )}
                                    </td>
                                    <td>
                                        {task.assignedToCurrentUser && (
                                            <button className="btn btn-outline-success btn-sm me-2" onClick={() => handleCompleteTask(task.id)} disabled={task.isCompleted} > Mark as completed</button>
                                        )}
                                        {task.assignedToCurrentUser && (
                                            <button className="btn btn-outline-primary btn-sm me-2" onClick={() => handleEditTask(task)} > Update </button>
                                        )}
                                        {!task.assignedToCurrentUser && <span className="text-muted small">View Only</span>}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}

            {activeTab === "assignment" && (
                <div className="container pt-3">
                    <div className="p-5 rounded-1 shadow-lg bg-white border">

                        {/* Heading */}
                        <h2 className="mb-2 fw-bold">Assignments</h2>
                        <p className="text-muted mb-5">
                            Assign or remove employees to/from my projects and tasks.
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
                                        {tasks
                                            .filter((task) => task.assignedToCurrentUser)
                                            .map((task) => (
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
                                        {employees.filter(emp => emp.email !== profile.email)
                                            .map((emp) => (
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
                                    {/*<button className="btn btn-outline-danger" onClick={handleRemoveEmployeeFromTask}>*/}
                                    {/*    ➖ Remove*/}
                                    {/*</button>*/}
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            )}
        </div>
    );
};

export default EmployeeDashboard;