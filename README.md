# Perceptron Text Classifier

Simple perceptron text classifier used for my class presentation. The program implements a [single layer perceptron](https://en.wikipedia.org/wiki/Perceptron), the simplest neural network. 
Each perceptron learns to distinguish a class from others and is fed input from .arff files(Atribute-Relation File Format). Note that the images shown below were made by me (**Zamfir Alexandru**) in [Apple Keynote](https://www.apple.com/keynote/).
<br></br>
![image](https://github.com/m0b-x/PerceptronTextClassifier/assets/72597190/e165f2a4-d188-460f-9faf-3f987d66b030)

## Why use the Single Layer Perceptron?

- it is easy to implement, serves as an introduction to neural networks;
- it makes use of parallelization, since each perceptron is independent from each other;
- it has a simple learning rule;

## Why not use the Single Layer Perceptron?

- Can only perfectly learn linearly separable data;
- Mixed (but generally good) results when learning non-linearly separable data;

## Measuring Methods / Evaluation Metrics

In order to measure the performance of our program, we will be using the [Confusion Matrix](https://en.wikipedia.org/wiki/Confusion_matrix).

![image](https://github.com/m0b-x/PerceptronTextClassifier/assets/72597190/0b456ba7-7b33-44f6-aca7-b33e72e87bb6)

### Relevant formulas:
- **Precision**: Measures the proportion of true positive predictions among all positive predictions made by the model.
- **Accuracy**: Measures the overall correctness of the model's predictions.
- **Recall (Sensitivity)**: Measures the proportion of true positive predictions among all actual positive instances.
- **Specificity**: Measures the proportion of true negative predictions among all actual negative instances.

## Results

![image](https://github.com/m0b-x/PerceptronTextClassifier/assets/72597190/ea864529-c7fc-41e6-9aea-602f70138160)

