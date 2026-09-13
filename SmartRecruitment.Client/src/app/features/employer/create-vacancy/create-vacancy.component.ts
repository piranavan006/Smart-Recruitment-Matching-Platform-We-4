import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-create-vacancy',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './create-vacancy.component.html',
  styleUrl: './create-vacancy.component.css'
})
export class CreateVacancyComponent {

  vacancyForm: FormGroup;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private router: Router
  ) {

    this.vacancyForm = this.fb.group({

      title: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ],

      description: [
        '',
        [
          Validators.required,
          Validators.minLength(20)
        ]
      ],

      location: [
        '',
        Validators.required
      ],

      jobType: [
        'Full Time',
        Validators.required
      ],

      salaryMin: [
        '',
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      salaryMax: [
        '',
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      experienceMin: [
        0,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      experienceMax: [
        0,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      applicationDeadline: [
        '',
        Validators.required
      ],

      requiredSkills: this.fb.array([
        this.createSkill()
      ])

    });
  }


  // Create one skill row
  createSkill(): FormGroup {
    return this.fb.group({

      skill: [
        '',
        Validators.required
      ],

      weight: [
        10,
        [
          Validators.required,
          Validators.min(1),
          Validators.max(100)
        ]
      ]

    });
  }


  // Required Skills FormArray
  get requiredSkills(): FormArray {
    return this.vacancyForm.get('requiredSkills') as FormArray;
  }


  // Add new skill
  addSkill(): void {
    this.requiredSkills.push(
      this.createSkill()
    );
  }


  // Remove skill
  removeSkill(index: number): void {

    if (this.requiredSkills.length > 1) {
      this.requiredSkills.removeAt(index);
    }

  }


  // Check experience range
  isExperienceInvalid(): boolean {

    const min =
      this.vacancyForm.get('experienceMin')?.value;

    const max =
      this.vacancyForm.get('experienceMax')?.value;

    return min !== null &&
           max !== null &&
           min !== '' &&
           max !== '' &&
           Number(min) > Number(max);
  }


  // Check salary range
  isSalaryInvalid(): boolean {

    const min =
      this.vacancyForm.get('salaryMin')?.value;

    const max =
      this.vacancyForm.get('salaryMax')?.value;

    return min !== null &&
           max !== null &&
           min !== '' &&
           max !== '' &&
           Number(min) > Number(max);
  }


  // Check deadline
  isDeadlineInvalid(): boolean {

    const deadline =
      this.vacancyForm.get('applicationDeadline')?.value;

    if (!deadline) {
      return false;
    }

    const selectedDate = new Date(deadline);
    const today = new Date();

    today.setHours(0, 0, 0, 0);

    return selectedDate < today;
  }


  // Submit vacancy
  onSubmit(): void {

    this.successMessage = '';
    this.errorMessage = '';

    if (this.vacancyForm.invalid) {

      this.vacancyForm.markAllAsTouched();

      this.errorMessage =
        'Please complete all required fields.';

      return;
    }


    if (this.isExperienceInvalid()) {

      this.errorMessage =
        'Maximum experience must be greater than or equal to minimum experience.';

      return;
    }


    if (this.isSalaryInvalid()) {

      this.errorMessage =
        'Maximum salary must be greater than or equal to minimum salary.';

      return;
    }


    if (this.isDeadlineInvalid()) {

      this.errorMessage =
        'Application deadline must be today or a future date.';

      return;
    }


    this.isSubmitting = true;


    // Temporary frontend test
    console.log(
      'Vacancy Data:',
      this.vacancyForm.value
    );


    setTimeout(() => {

      this.isSubmitting = false;

      this.successMessage =
        'Vacancy created successfully!';

      this.vacancyForm.reset({

        jobType: 'Full Time',

        experienceMin: 0,

        experienceMax: 0

      });

    }, 800);

  }


  // Easy access for template validation
  isInvalid(controlName: string): boolean {

    const control =
      this.vacancyForm.get(controlName);

    return !!(
      control &&
      control.invalid &&
      control.touched
    );

  }

}