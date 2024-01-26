
# PetConnect
Team Members:

 - Ioana Cîlțea 342
 - Diana Clinciu 344
 - Iulia Rotaru 344
 - Antonia State 344
 - Cristina Boboc 344


## Intermediate deliverable Party Team



## Problem statement
The recognized problem revolves around the growing population of stray animals and the difficulty potential adopters encounter in identifying suitable pets. This highlights a distinct necessity to simplify the pet adoption process, guaranteeing that individuals seeking adoption have convenient access to comprehensive information, images, and particulars about animals available for adoption. Our proposed solution involves the development of a dedicated pet adoption website, serving as a centralized platform for adoption organizations and prospective adopters. The primary beneficiaries of this solution encompass prospective adopters, adoption organizations, and, critically, the animals awaiting homes.

## Product features and functionalities

| Feature/Functionality  | Status | Description |
| ------------- | ------------- | ------------- |
| User Registration  | Not started |Collect and validate user's basic information, such as name, email address, and password.|
| User Authentication | Not started | Allow users to log in using their email address or username and secure password. |
| Announcement Adoption| Not started |  Empower pet owners to create adoption announcements, providing essential details like pet name, species, breed, age, size, and a brief description. Allow the upload of multiple images to showcase the pet's appearance. |
| Pet Listings| Not started | Present a comprehensive list of adoptable pets, offering search and filter options (by species, breed, etc.), along with detailed profiles featuring images and descriptions for each pet.|
| Adoption Process | Not started | Facilitate adoption applications and provide a system for tracking adoption statuses |
| Admin Panel | Not started | Manage pet listings with functions to add, edit, and delete entries, as well as review and approve/reject adoption applications. |
| Messaging System | Not started | Enable communication between users and admins for adoption inquiries, with notifications for new messages. |
| User Profile | Not started |  Provide personal profiles for users, allowing them to update their contact information. |
| Comments on Announcements | Not started | The ability for users to leave comments on adoption announcements to share experiences, ask questions, or provide additional information. |
| Legal Pages | Not started | Terms of Service, Privacy Policy, and other legal information. |



## Product Roadmap
![roadmap](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/7f62798d-88db-47ce-bb0f-6a6df75da9f5)

## Non-functional requirements
Non-functional Requirements (NFRs) are designed to describe the qualities of a system, various attributes of systems not directly linked to their functionality. These attributes do not explain what the system does, but they focus on how effectively those functions are performed.

1. **Performance:**
   - The solution should be able to store the data of up to 25000 users and 2000 pet adoption ads.
   - The maximum page load time should not exceed 3 seconds.
   - The list of announcements should be loaded in less than 5 seconds, with a maximum of 20 announcements per page.

2. **Availability:**
   - The website should be available between 7 AM – 12:00 AM to allow users to access information and interact with announcements.

3. **Integrity:**
   - Information about animals (descriptions, images, etc.) must be accurate and protected against unauthorized modifications.
   - Only the author of the announcement can edit or delete information regarding the announcement.
   - The system shall ensure data integrity by implementing robust measures against SQL injection attacks.

4. **Security:**
   - Passwords must have at least 10 characters in length and contain at least one digit, one lowercase letter, and one uppercase letter.
   - The system will not retain user passwords in their original form; instead, passwords will undergo hashing before transmission to the server.

5. **Compatibility:**
   - The website should be compatible with the latest versions of popular browsers such as Google Chrome, Mozilla Firefox, Safari, and Microsoft Edge.
   - It should be responsive and functional on various devices, including desktops, tablets, and smartphones.

6. **Usability:**
   - Ensure the user interface is intuitive, user-friendly, and facilitates easy navigation and use.
   - Measure user-friendliness through user testing and feedback, aiming for a satisfaction rate of at least 90%.
   - Provide clear feedback on user actions, system responses, and status updates, aiming for a clarity rating of 95%.

7. **Maintainability:**
   - Provide detailed documentation for developers to facilitate streamlined maintenance.
   - Enforce standardized coding practices to enhance code readability and maintainability, including adhering to coding conventions, naming conventions, and design patterns.
   - Implement a robust version control system (e.g., Git) and encourage regular commits with meaningful commit messages.
## Customer Journey
1. **User Personas**
[UserPersonas.pdf](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/files/13467893/UserPersonas.pdf)

![userpersona1](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/e95f4b79-528e-428e-bf21-de3ee8f891a7)

![userpersona2](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/df169d1e-b44a-4b61-aef3-b2eaf7b52fb2)



3. **User Journey Map**
   [UserJourneyMap.pdf](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/files/13467891/UserJourneyMap.pdf)
![userjouneymap](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/244ab9e6-334e-4b10-b118-2cb2c8f365d0)

## Activity Diagram
![Diagram](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/d92f959a-73d7-4c6e-acf6-a3d7b9659bf5)



## User stories
## User stories with acceptance criteria [UserStories.pdf](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/files/13467923/UserStories.pdf)

### 1. User Authentication:

As a user, I want a unified and secure authentication process that includes a straightforward login and registration flow so that I can quickly access and manage my account.


### 2. Profile Setup:

As a new user, I want the option to set up my profile immediately after registration so that I can personalize my experience on the platform. 


### 3. "About Us" Section:

As a curious visitor to the website, I want access to a comprehensive "About Us" section that provides detailed insights into the platform so that I can enhance my understanding and trust in the platform.


### 4. Blog articles:

As a website visitor interested in pet care and adoption, I want to see blog articles featured on the homepage, so that I can easily access informative content about pets.

### 5. Testimonials:

As a user, I want to read  testimonials from other individuals so that I can gain valuable insights into their experiences with the platform.

### 6. Terms and Conditions Access:

As a user accessing the pet adoption platform, I want to be presented with a Terms and Conditions pop-up, so that I am aware of and agree to the terms before using the platform's services.

### 7. Contact Form:

As a visitor, I want to easily find and access a contact form so that I can have a straightforward method to reach out to the platform's team for general inquiries or assistance.

### 8. Animal Description:

As a visitor, I want to easily access detailed descriptions of animals, including their characteristics and relevant information so that I can make informed decisions about potential adoptions.

### 9. Adoption Announcements Lifecycle:

As a pet owner, I want to seamlessly create, update, and remove adoption announcements so that I can ensure the information displayed is accurate and relevant, facilitating the adoption process.

### 10. Explore and Filter Adoption Announcements:

As a user in search of a pet, I want the capability to explore and filter adoption announcements based on specific criteria so that I can easily find animals that match my preferences and lifestyle.

### 11. Leave Comments on Announcements:

As a potential adopter, I want the capability to leave comments on adoption announcements so that I can inquire about additional details and engage with other users in the community.


### 12. Messaging Pet Owners:

As a user, I want to send messages to the owners of pets available for adoption so that I can inquire about details regarding the animal or express my interest in adopting.


### 13. Pet Adoption Application

As a potential pet adopter excited to provide a loving home, I want to submit an adoption application easily and efficiently, so that I can express my interest in adopting a furry companion.

### 14. Content Moderation:
As an administrator, I want the capability to review and moderate user-generated content, including adoption announcements, comments, and posts, so that I can maintain a positive and safe community environment.

### 15. Role-Based Permissions:
  
As an administrator, I want the capability to assign different roles and permissions to staff members involved in platform management so that I can delegate responsibilities effectively.

## Backlog
Link Trello: https://trello.com/b/uqZEI5tk/pet-adoption



![backlog](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/4656f5a2-dca7-4d1d-a3bf-b4e770fff3c3)

Backlog estimation:

![backlogEstimation](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/assets/92121071/2b765935-d5e9-4d0b-95be-fb4a5e283728)


## First Sprint Report [First Sprint Report.pdf](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/files/13956501/First.Sprint.Report.pdf)

## Second Sprint Report [Sprint Report 2.pdf](https://github.com/inginerie-software-2023-2024/proiect-inginerie-software-party/files/14063026/Sprint.Report.2.pdf)

