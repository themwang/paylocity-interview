import React, { useState } from "react";
import { useForm, useFieldArray } from "react-hook-form";
import axios from "axios";

export function DeductionCalculator() {
  const {
    register,
    control,
    handleSubmit,
    formState: { errors },
  } = useForm();
  const { fields, append, remove } = useFieldArray({
    control,
    name: "dependents",
  });

  const onSubmit = async (data) => {
    //TODO: get the API url from an env file

    // Save the employee
    const result = await axios.post("https://localhost:7083/employees", data);

    if ((result.status = "200")) {
      // If the save succeeds, query the API again to get the deduction results
      const deductionQueryResult = await axios.get(
        `https://localhost:7083/employee/${result.data}/payroll-deduction`
      );
      setDeductionResult(deductionQueryResult.data);
    }
  };

  const inputForm = (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="form-group row mb-3">
        <label className="col-lg-4 col-form-label form-control-label">
          Employee Name
        </label>
        <div className="col-lg-8">
          <input
            name="employeeName"
            type="text"
            {...register("employeeName", {
              required: true,
              maxLength: 80,
            })}
            className={`form-control ${
              errors.employeeName ? "is-invalid" : ""
            }`}
          />
          <div className="invalid-feedback">{errors.employeeName?.message}</div>
        </div>
      </div>

      {fields.map((item, index) => (
        <div key={item.id} className="form-group row mb-3">
          <label className="col-lg-4 col-form-label form-control-label">
            Dependent {index + 1}
          </label>
          <div className="col-lg-8">
            <input
              name="name"
              type="text"
              placeholder="Name"
              {...register(`dependents.${index}.name`)}
              className={`form-control mb-3 ${errors.name ? "is-invalid" : ""}`}
            />

            <select
              placeholder="Dependent Type"
              {...register(`dependents.${index}.type`)}
              className="form-control mb-3"
            >
              <option>Spouse</option>
              <option>Child</option>
              <option>Other</option>
            </select>

            <button
              className="btn btn-secondary float-right"
              type="button"
              onClick={() => remove(index)}
            >
              Delete
            </button>
          </div>
        </div>
      ))}

      <div className="form-group row">
        <label className="col-lg-3 col-form-label form-control-label"></label>
        <div className="col-lg-9">
          <input
            type="button"
            className="btn btn-secondary mx-1"
            onClick={() => append({ name: "", type: "" })}
            value="Add Dependent"
          />

          <input className="btn btn-primary mx-1" type="submit" />
        </div>
      </div>
    </form>
  );

  const [deductionResult, setDeductionResult] = useState(null);

  const content =
    deductionResult === null ? (
      inputForm
    ) : (
      <div>
        <div className="row">
          <div className="col-lg-9">Total Deduction Per Year: </div>
          <div className="col-lg-3">
            ${deductionResult.totalDeductionPerYear}
          </div>
        </div>
        <div className="row">
          <div className="col-lg-9">Total Deduction Per Pay Check: </div>
          <div className="col-lg-3">
            ${deductionResult.totalDeductionPerPayCheck}
          </div>
        </div>
        <div className="row">
          <div className="col-lg-9">Total Net Pay Per Pay Check: </div>
          <div className="col-lg-3">
            ${deductionResult.totalNetPayPerPayCheck}
          </div>
        </div>
      </div>
    );

  return (
    <div className="row justify-content-center">
      <div className="col-md-6">
        <div className="card-header">
          <h3 className="mb-0">Deduction Calculator</h3>
        </div>

        <div className="card-body">{content}</div>
      </div>
    </div>
  );
}
